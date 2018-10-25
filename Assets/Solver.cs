using UnityEngine;

public class Solver : MonoBehaviour
{
    public FingerController fingerController;
    public UIController uiController;

    float x = 45;
    float fingerslideMcpjGuess = 30;
    float fingerslidePipjGuess = -20;
    bool fingerslideActive = false;
    float slideInterval = 1;

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
            
            Debug.Log(xError + yError);

            if (iterationCounter == 500 || xError + yError < errorThreshold)
            {
                uiController.setSliderWholeNumbers(false);
                uiController.independent.isOn = false;
                uiController.updatePPSlider(mcpjGuess);
                fingerController.PPChanged(mcpjGuess);
                uiController.updateIPSlider(pipjGuess);
                fingerController.IPChanged(pipjGuess, false);

                Debug.Log("Iterations: " + iterationCounter);

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

    public void JacobianIterative(float mcpjGuess, float pipjGuess, float desiredX)
    {
        float degreeToRadianConstant = Mathf.PI / 180;
        float desiredY = -17.5f;

        int iterationCounter = 0;

        float errorThreshold = 0.05f;

        float currentX;
        float currentY;

        while (true)
        {
            currentX = 39.8f * Mathf.Cos(mcpjGuess * degreeToRadianConstant) + 22.4f * Mathf.Cos((mcpjGuess + pipjGuess) * degreeToRadianConstant) + 15.8f * Mathf.Cos((mcpjGuess + 5 * pipjGuess / 3) * degreeToRadianConstant);
            currentY = 39.8f * Mathf.Sin(mcpjGuess * degreeToRadianConstant) + 22.4f * Mathf.Sin((mcpjGuess + pipjGuess) * degreeToRadianConstant) + 15.8f * Mathf.Sin((mcpjGuess + 5 * pipjGuess / 3) * degreeToRadianConstant);

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


                Debug.Log("Iterations: " + iterationCounter);

                fingerslideMcpjGuess = mcpjGuess;
                fingerslidePipjGuess = pipjGuess;

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

    public void fingerslide()
    {
        fingerslideActive = true;
        slideInterval = uiController.getSlideInterval();
    }


    void Update()
    {
        if (fingerslideActive)
        {
            if (x <= 76)
            {
                JacobianIterative(fingerslideMcpjGuess, fingerslidePipjGuess, x);
                
                x += slideInterval;
            }
            else
            {
                fingerslideActive = false;
                x = 45;
                fingerslideMcpjGuess = 30;
                fingerslidePipjGuess = -20;
            }
        }
    }

	// Use this for initialization
	void Start ()
    {
       
	}
}
