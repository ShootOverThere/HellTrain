using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Slider health_slider;
    public Gradient gradient;
    public Image fill;

    public void SetMaxHealth(int health)
    {
        health_slider.maxValue = health;
        health_slider.value = health;


        gradient.Evaluate(1f);
        fill.color = gradient.Evaluate(health_slider.normalizedValue);
    }
    public void SetHealth(int health)
    {
        health_slider.value = health;

        fill.color = gradient.Evaluate(health_slider.normalizedValue);
    }
}
