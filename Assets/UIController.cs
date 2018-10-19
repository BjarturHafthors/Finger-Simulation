using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public FingerController fingerController;
    public Slider PPSlider;
    public Slider IPSlider;
    public Slider DPSlider;
    public Text PIPJValue;
    public Text DIPJValue;
    public Text fingertipValue;
    public Toggle independent;
    public Text DPMinText;

	// Use this for initialization
	void Start ()
    {
        PPSlider.maxValue = 60;
        PPSlider.minValue = -60;
        IPSlider.maxValue = 0;
        IPSlider.minValue = -120;
        DPSlider.maxValue = 0;
        DPSlider.minValue = -120;
	}
	
    public void PPChanged()
    {
        fingerController.PPChanged(PPSlider.value);
    }

    public void IPChanged()
    {
        fingerController.IPChanged(IPSlider.value, independent.isOn);
    }

    public void DPChanged()
    {
        fingerController.DPChanged(DPSlider.value, independent.isOn);
    }

    public void updateIPSlider(float value)
    {
        IPSlider.value = value;
    }

    public void updateDPSlider(float value)
    {
        DPSlider.value = value;
    }

    public void UpdatePIPJValue(float x, float y)
    {
        PIPJValue.text = "(" + x.ToString("F1") + "," + y.ToString("F1") + ")";
    }

    public void UpdateDIPJValue(float x, float y)
    {
        DIPJValue.text = "(" + x.ToString("F1") + "," + y.ToString("F1") + ")";
    }

    public void UpdateFingertipValue(float x, float y)
    {
        fingertipValue.text = "(" + x.ToString("F1") + "," + y.ToString("F1") + ")";
    }

    public void independentToggled()
    {
        if (independent.isOn)
        {
            DPSlider.minValue = -120;
            DPMinText.text = "-120";
        }
        else
        {
            float newDPValue = IPSlider.value * 2 / 3;
            DPSlider.minValue = -80;
            DPSlider.value = newDPValue;
            fingerController.DPChanged(newDPValue, false);
            DPMinText.text = "-80";
        }
    }
}
