using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Powerbar : MonoBehaviour
{    public Slider power_slider;
    public Gradient gradient;
    public Image fill;

    public void SetMaxHealth(int power)
    {
        power_slider.maxValue = power;
        power_slider.value = power;


        gradient.Evaluate(1f);
        fill.color = gradient.Evaluate(power_slider.normalizedValue);
    }
    public void SetPower(int power)
    {
        power_slider.value = power;

        fill.color = gradient.Evaluate(power_slider.normalizedValue);
    }
}
