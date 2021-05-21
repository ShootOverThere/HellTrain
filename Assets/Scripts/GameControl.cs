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

    public AreaEffector2D wind_field;
    public Text wind_txt;
    

    //[HideInInspector]
    public GameObject currentPlayer;
    
    // Start is called before the first frame update
    void Awake()
    {
    }

    // Update is called once per frame
    public void ChangePlayer()
    {
    }

    public void SetPlayer()
    {
    }

    public void WindChange()
    {
    }
}