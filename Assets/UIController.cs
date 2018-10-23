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

    public Text PPValue;
    public Text IPValue;
    public Text DPValue;

    public InputField xValue;
    public InputField yValue;
    public InputField mcpjGuess;
    public InputField pipjGuess;
    public InputField interval;

	// Use this for initialization
	void Start ()
    {
        PPSlider.maxValue = 60;
        PPSlider.minValue = -60;
        IPSlider.maxValue = 0;
        IPSlider.minValue = -120;
        DPSlider.maxValue = 0;
        DPSlider.minValue = -120;

        PPValue.text = PPSlider.value.ToString();
        IPValue.text = IPSlider.value.ToString();
        DPValue.text = DPSlider.value.ToString();
	}
	
    public void PPChanged()
    {
        fingerController.PPChanged(PPSlider.value);
        PPValue.text = PPSlider.value.ToString();
    }

    public void IPChanged()
    {
        fingerController.IPChanged(IPSlider.value, independent.isOn);
        IPValue.text = IPSlider.value.ToString();
    }

    public void DPChanged()
    {
        fingerController.DPChanged(DPSlider.value, independent.isOn);
        DPValue.text = DPSlider.value.ToString();
    }

    public void updatePPSlider(float value)
    {
        PPSlider.value = value;
        PPValue.text = value.ToString();
    }

    public void updateIPSlider(float value)
    {
        IPSlider.value = value;
        IPValue.text = value.ToString();
    }

    public void updateDPSlider(float value)
    {
        DPSlider.value = value;
        DPValue.text = value.ToString();
    }

    public void setSliderWholeNumbers(bool value)
    {
        if (value)
        {
            PPSlider.wholeNumbers = true;
            IPSlider.wholeNumbers = true;
            DPSlider.wholeNumbers = true;
        }
        else
        {
            PPSlider.wholeNumbers = false;
            IPSlider.wholeNumbers = false;
            DPSlider.wholeNumbers = false;
        }
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

    public bool isIndependent()
    {
        return independent.isOn;
    }

    public float getXInput()
    {
        return float.Parse(xValue.text);
    }

    public float getYInput()
    {
        return float.Parse(yValue.text);
    }

    public float getMCPJGuess()
    {
        return float.Parse(mcpjGuess.text);
    }

    public float getSlideInterval()
    {
        return float.Parse(interval.text);
    }

    public float getPIPJGuess()
    {
        return float.Parse(pipjGuess.text);
    }
}
