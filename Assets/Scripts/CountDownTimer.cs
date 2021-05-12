using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour
{
    public GameControl gc;

    [HideInInspector]
    public float currentTime = 0f;
    public float startingTime = 10f;
    public Color hurry_color;
    public Color not_hurry_color;
    bool isTimeFlow = true;
    
    
    [SerializeField]
    Text countdownText;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = startingTime;
        countdownText.text = ((int)currentTime).ToString("0") + "초";
        countdownText.color = not_hurry_color;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTimeFlow)
        {
            if (currentTime >= 0)
            {
                if (currentTime <= 5)
                    countdownText.color = hurry_color;
                currentTime -= 1 * Time.deltaTime;
                countdownText.text = ((int)currentTime).ToString("0") + "초";
                
            }
            else
            {
                gc.ChangePlayer();
            }
        }
    }

    public void ResetTimer()
    {
        currentTime = startingTime;
        countdownText.text = ((int)currentTime).ToString("0") + "초";
        countdownText.color = not_hurry_color;
        isTimeFlow = true;
    }

    public void TimeflowStop()
    {
        isTimeFlow = false;
    }
}
