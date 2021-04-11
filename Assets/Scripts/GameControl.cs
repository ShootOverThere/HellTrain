using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    [SerializeField]
    public GameObject[] players;

    public GameObject canvas_for_timer;
    

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
    }

    public GameObject SetPlayer()
    {
        return currentPlayer;
    }

}