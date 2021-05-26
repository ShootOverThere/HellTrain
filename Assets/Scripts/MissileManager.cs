using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileManager : MonoBehaviour
{
    public GameObject missile_prefab;
    public int missileCount = 0;
    List<GameObject> missileList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        missileInit(20);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void missileInit(int count)
    {
        for(int i=0; i<count; i++)
        {
            GameObject obj = Instantiate(missile_prefab) as GameObject;
            missileList.Add(obj);
            missileList[i].SetActive(false);
        }
    }
}
