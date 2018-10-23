using UnityEngine;

public class Solver : MonoBehaviour
{
    public FingerController fingerController;
    public UIController uiController;

    public void BruteSolve(float x)
    {
        //float x = uiController.getXInput();
        //float y = uiController.getYInput();

        float y = -17.5f;
        //for (float x = 45f; x <= 76f; x++)
        {
            float errorX = 999999999;
            float errorY = 999999999;

            float bestI = -60;
            float bestJ = -120;
            float bestK = -120;

            if (uiController.isIndependent())
            {
                for (int i = -60; i <= 60; i++)
                {
                    for (int j = -120; j <= 0; j++)
                    {
                        for (int k = -120; k <= 0; k++)
                        {
                            float newX = fingerController.getXFromDegrees(i, j, k);
                            float newY = fingerController.getYFromDegrees(i, j, k);

                            if (Mathf.Abs(x - newX) + Mathf.Abs(y - newY) < errorX + errorY)
                            {
                                bestI = i;
                                bestJ = j;
                                bestK = k;
                                errorX = Mathf.Abs(x - newX);
                                errorY = Mathf.Abs(y - newY);
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = -60; i <= 60; i++)
                {
                    for (int j = -120; j <= 0; j++)
                    {
                        float newX = fingerController.getXFromDegrees(i, j, 2 * j / 3);
                        float newY = fingerController.getYFromDegrees(i, j, 2 * j / 3);

                        if (Mathf.Abs(x - newX) + Mathf.Abs(y - newY) < errorX + errorY)
                        {
                            bestI = i;
                            bestJ = j;
                            bestK = 2 * j / 3;
                            errorX = Mathf.Abs(x - newX);
                            errorY = Mathf.Abs(y - newY);
                        }
                    }
                }
            }

            fingerController.PPChanged(bestI);
            uiController.updatePPSlider(bestI);
            fingerController.IPChanged(bestJ, uiController.isIndependent());
            uiController.updateIPSlider(bestJ);
            fingerController.DPChanged(bestK, uiController.isIndependent());
            uiController.updateDPSlider(bestK);
        }

        previousX++;
    }

    public void JacobianIterative()
    {
        float degreeToRadianConstant = Mathf.PI / 180;

        float mcpjGuess = uiController.getMCPJGuess();
        float pipjGuess = uiController.getPIPJGuess();

        float desiredX = uiController.getXInput();
        float desiredY = uiController.getYInput();

        int iterationCounter = 0;

        float errorThreshold = 0.05f;

        float currentX;
        float currentY;

        while (true)
        {
            currentX = 39.8f * Mathf.Cos(mcpjGuess * degreeToRadianConstant) + 22.4f * Mathf.Cos((mcpjGuess + pipjGuess) * degreeToRadianConstant) + 15.8f * Mathf.Cos((mcpjGuess + 5*pipjGuess/3) * degreeToRadianConstant);
            currentY = 39.8f * Mathf.Sin(mcpjGuess * degreeToRadianConstant) + 22.4f * Mathf.Sin((mcpjGuess + pipjGuess) * degreeToRadianConstant) + 15.8f * Mathf.Sin((mcpjGuess + 5*pipjGuess/3) * degreeToRadianConstant);

            float xError = Mathf.Abs(desiredX - currentX);
            float yError = Mathf.Abs(desiredY - currentY);

            if (iterationCounter == 500 || xError + yError < errorThreshold)
            {
                uiController.setSliderWholeNumbers(false);
                uiController.independent.isOn = false;
                uiController.updatePPSlider(mcpjGuess);
                fingerController.PPChanged(mcpjGuess);
                uiController.updateIPSlider(pipjGuess);
                fingerController.IPChanged(pipjGuess, false);

                Debug.Log("Iteration: " + iterationCounter);
                Debug.Log("MCPJ: " + mcpjGuess);
                Debug.Log("PIPJ: " + pipjGuess);
                Debug.Log("X: " + currentX + ", Y: " + currentY);

                return;
            }

            float[] jacobian = new float[4];
            jacobian[0] = 39.8f * -Mathf.Sin(mcpjGuess * degreeToRadianConstant) + 22.4f * -Mathf.Sin((mcpjGuess + pipjGuess) * degreeToRadianConstant) + 15.8f * -Mathf.Sin((pipjGuess + 5 * pipjGuess / 3) * degreeToRadianConstant);
            jacobian[1] = 22.4f * -Mathf.Sin((mcpjGuess + pipjGuess) * degreeToRadianConstant) + 15.8f * -Mathf.Sin((pipjGuess + 5 * pipjGuess / 3) * degreeToRadianConstant);
            jacobian[2] = 39.8f * Mathf.Cos(mcpjGuess * degreeToRadianConstant) + 22.4f * Mathf.Cos((mcpjGuess + pipjGuess) * degreeToRadianConstant) + 15.8f * Mathf.Cos((pipjGuess + 5 * pipjGuess / 3) * degreeToRadianConstant);
            jacobian[3] = 22.4f * Mathf.Cos((mcpjGuess + pipjGuess) * degreeToRadianConstant) + 15.8f * Mathf.Cos((pipjGuess + 5 * pipjGuess / 3) * degreeToRadianConstant);

            float determinant = 1 / (jacobian[0] * jacobian[3] - jacobian[1] * jacobian[2]);

            float[] inverse = new float[4];
            inverse[0] = determinant * jacobian[3];
            inverse[1] = determinant * -jacobian[1];
            inverse[2] = determinant * -jacobian[2];
            inverse[3] = determinant * jacobian[0];

            // Change guesses
            mcpjGuess = mcpjGuess + inverse[0] * (desiredX - currentX) + inverse[1] * (desiredY - currentY);
            pipjGuess = pipjGuess + inverse[2] * (desiredX - currentX) + inverse[3] * (desiredY - currentY);

            iterationCounter++;
        }
    }

	// Use this for initialization
	void Start ()
    {
       
	}

    float x = 44;
    float previousX = 44;

	// Update is called once per frame
	void Update ()
    {
        /*if (x == previousX && x <= 76)
        {
            x++;
            BruteSolve(x);
        }*/
    }
}
