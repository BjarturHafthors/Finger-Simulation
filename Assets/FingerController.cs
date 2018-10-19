using UnityEngine;

public class FingerController : MonoBehaviour
{
    public GameObject pp;
    public GameObject ip;
    public GameObject dp;
    public GameObject mcpj;
    public GameObject pipj;
    public GameObject dipj;
    public GameObject fingertip;

    public float ppSize;
    public float ipSize;
    public float dpSize;

    public UIController uiController;
    
    // Use this for initialization
    void Start ()
    {
        /*float x = pp.GetComponent<SpriteRenderer>().bounds.size.x;
        pp.transform.position = new Vector3(pp.transform.position.x + pp.GetComponent<SpriteRenderer>().bounds.size.x / 2, pp.transform.position.y);

        pipj.transform.position = new Vector3(pipj.transform.position.x + pp.GetComponent<SpriteRenderer>().bounds.size.x / 2, pipj.transform.position.y);

        x = ip.GetComponent<SpriteRenderer>().bounds.size.x;
        ip.transform.position = new Vector3(ip.transform.position.x + ip.GetComponent<SpriteRenderer>().bounds.size.x / 2, ip.transform.position.y);
        
        dipj.transform.position = new Vector3(dipj.transform.position.x + ip.GetComponent<SpriteRenderer>().bounds.size.x / 2, dipj.transform.position.y);

        x = dp.GetComponent<SpriteRenderer>().bounds.size.x;
        dp.transform.position = new Vector3(dp.transform.position.x + dp.GetComponent<SpriteRenderer>().bounds.size.x / 2, dp.transform.position.y);
        
        fingertip.transform.position = new Vector3(fingertip.transform.position.x + dp.GetComponent<SpriteRenderer>().bounds.size.x / 2, fingertip.transform.position.y);*/

        updateFingertip();
    }

    public void PPChanged(float value)
    {
        mcpj.transform.eulerAngles = new Vector3(0, 0, value);

        updatePIPJ();
        updateDIPJ();
        updateFingertip();
    }

    public void IPChanged(float value, bool independent)
    {
        pipj.transform.localEulerAngles = new Vector3(0, 0, value);

        if (!independent)
        {
            float dpValue = (2 * value) / 3;
            dipj.transform.localEulerAngles = new Vector3(0, 0, dpValue);
            uiController.updateDPSlider(dpValue);
        }

        updateDIPJ();
        updateFingertip();
    }

    public void DPChanged(float value, bool independent)
    {
        dipj.transform.localEulerAngles = new Vector3(0, 0, value);

        if (!independent)
        {
            float ipValue = (3 * value) / 2; 
            pipj.transform.localEulerAngles = new Vector3(0, 0, ipValue);
            uiController.updateIPSlider(ipValue);
        }
        
        updateFingertip();
    }

    private void updatePIPJ()
    {
        uiController.UpdatePIPJValue(pipj.transform.position.x, pipj.transform.position.y);
    }

    private void updateDIPJ()
    {
        uiController.UpdateDIPJValue(dipj.transform.position.x, dipj.transform.position.y);
    }

    private void updateFingertip()
    {
        /*float degreeToRadianConsant = Mathf.PI / 180;
        float x = ppSize * Mathf.Cos(mcpj.transform.eulerAngles.z * degreeToRadianConsant) + ipSize * Mathf.Cos(pipj.transform.eulerAngles.z * degreeToRadianConsant) + dpSize * Mathf.Cos(dipj.transform.eulerAngles.z * degreeToRadianConsant);
        float y = ppSize * Mathf.Sin(mcpj.transform.eulerAngles.z * degreeToRadianConsant) + ipSize * Mathf.Sin(pipj.transform.eulerAngles.z * degreeToRadianConsant) + dpSize * Mathf.Sin(dipj.transform.eulerAngles.z * degreeToRadianConsant);

        Debug.Log("Equations: (" + x.ToString("F1") + "," + y.ToString("F1") + ")");
        Debug.Log("Actual: (" + fingertip.transform.position.x.ToString("F1") + "," + fingertip.transform.position.y.ToString("F1") + ")");*/

        uiController.UpdateFingertipValue(fingertip.transform.position.x, fingertip.transform.position.y);
    }
}
