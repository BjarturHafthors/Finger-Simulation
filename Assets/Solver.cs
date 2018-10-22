using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solver : MonoBehaviour
{
    public FingerController fingerController;
    public UIController uiController;

    public void BruteSolve()
    {
        float x = uiController.getXInput();
        float y = uiController.getYInput();

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
                    float newX = fingerController.getXFromDegrees(i, j, 2*j/3);
                    float newY = fingerController.getYFromDegrees(i, j, 2*j/3);

                    if (Mathf.Abs(x - newX) + Mathf.Abs(y - newY) < errorX + errorY)
                    {
                        bestI = i;
                        bestJ = j;
                        bestK = 2*j/3;
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

    public void JacobianIterative()
    {
        float degreeToRadianConstant = Mathf.PI / 180;

        float mcpjGuess = 0;
        float pipjGuess = 0;
        float dipjGuess = 0;

        float desiredX = 60;
        float desiredY = -10;

        int iterationCounter = 0;

        float errorThreshold = 0.1f;

        float currentX;
        float currentY;

        while (true)
        {
            currentX = 39.8f * Mathf.Cos(mcpjGuess * degreeToRadianConstant) + 22.4f * Mathf.Cos((mcpjGuess + pipjGuess) * degreeToRadianConstant) + 15.8f * Mathf.Cos((mcpjGuess + pipjGuess + dipjGuess) * degreeToRadianConstant);
            currentY = 39.8f * Mathf.Sin(mcpjGuess * degreeToRadianConstant) + 22.4f * Mathf.Sin((mcpjGuess + pipjGuess) * degreeToRadianConstant) + 15.8f * Mathf.Sin((mcpjGuess + pipjGuess + dipjGuess) * degreeToRadianConstant);

            if (iterationCounter == 25 || Mathf.Abs(desiredX - currentX) + Mathf.Abs(desiredY - currentY) < errorThreshold)
            {
                // Return solution
            }

            float oldMcpjGuess = mcpjGuess;
            float oldPipjGuess = pipjGuess;
            float oldDipjGuess = dipjGuess;

            // Change guesses
            mcpjGuess = 39.8f * -Mathf.Sin(oldMcpjGuess) + 22.4f * -Mathf.Sin(oldMcpjGuess + oldPipjGuess) + 15.8f * - Mathf.Sin(oldMcpjGuess + oldPipjGuess + oldDipjGuess);
            pipjGuess = 22.4f * -Mathf.Sin(oldMcpjGuess + oldPipjGuess) + 15.8f * -Mathf.Sin(oldMcpjGuess + oldPipjGuess + oldDipjGuess);
            dipjGuess = 15.8f * -Mathf.Sin(oldMcpjGuess + oldPipjGuess + oldDipjGuess);


            iterationCounter++;
        }
    }

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
