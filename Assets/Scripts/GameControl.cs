using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    [SerializeField]
    public GameObject[] players;
    public GameObject canvas_for_timer;
    public GameObject background;

    AreaEffector2D wind_field;
    public Text wind_txt;
    

    //[HideInInspector]
    public GameObject currentPlayer;
    
    // Start is called before the first frame update
    void Awake()
    {
        for(int i=0; i<players.Length; i++)
        {
            players[i].GetComponent<playerAiming>().isPlaying = false;
        }

        currentPlayer = players[0];
        currentPlayer.GetComponent<playerAiming>().isPlaying = true;

        wind_field = background.GetComponent<AreaEffector2D>();
        WindChange();
    }

    // Update is called once per frame
    public void ChangePlayer()
    {
        currentPlayer.GetComponent<playerAiming>().isPlaying = false;
        if (currentPlayer == players[0])
            currentPlayer = players[1];
        else if(currentPlayer == players[1])
            currentPlayer = players[0];
        currentPlayer.GetComponent<playerAiming>().isPlaying = true;

        CameraFollow.target = currentPlayer.transform;
        canvas_for_timer.GetComponent<CountDownTimer>().ResetTimer();

        WindChange();
    }

    public GameObject SetPlayer()
    {
        return currentPlayer;
    }

    public void WindChange()
    {
        int pow = Random.Range(-10, 10);
        Debug.Log("wind power:" + pow);
        string powerString = "";
        for(int i = 0; i<Mathf.Abs(pow); i++)
        {
            powerString += "-";
        }
        if(pow > 0)
        {
            wind_field.forceAngle = 0;
            wind_field.forceMagnitude = Mathf.Abs(pow);
            wind_txt.text = "바람\n" + powerString + ">";
        }
        else if(pow < 0)
        {
            wind_field.forceAngle = 180;
            wind_field.forceMagnitude = Mathf.Abs(pow);
            wind_txt.text = "바람\n" + "<" + powerString;
        }
        else
        {
            wind_field.forceAngle = 0;
            wind_field.forceMagnitude = Mathf.Abs(pow);
            wind_txt.text = "바람\n" + "-";
        }
    }

}