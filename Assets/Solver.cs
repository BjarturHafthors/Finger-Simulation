using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solver : MonoBehaviour
{
    public FingerController fingerController;
    public UIController uiController;

    public void Solve()
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

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
