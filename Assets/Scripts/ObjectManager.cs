using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectManager : MonoBehaviour
{
    public static TankAgent parent_tank;
    public static ObjectManager instance;
    public GameObject missile_prefab;
    List<GameObject> missiles = new List<GameObject>();
    public static int missile_count = 0;
    private void Awake()
    {
        if(ObjectManager.instance == null)
        {
            ObjectManager.instance = this;
        }
    }
    void Start()
    {
        CreateMissile(20);
    }
    void CreateMissile(int missileCount)
    {
        for(int i=0; i< missileCount; i++)
        {
            GameObject new_missile = Instantiate(missile_prefab) as GameObject;
            new_missile.transform.parent = transform;
            new_missile.SetActive(false);
            new_missile.GetComponent<missile>().parent_tank = parent_tank;
            missiles.Add(new_missile);
        }
    }
    public GameObject GetMissile(SpriteRenderer aimSprite)
    {
        GameObject reqMissile = null;
        for(int i=0; i< missiles.Count; i++)
        {
            if(missiles[i].activeSelf == false)
            {
                reqMissile = missiles[i];
                break;
            }
        }
        if(reqMissile == null)
        {
            GameObject new_missile = Instantiate(missile_prefab) as GameObject;
            new_missile.transform.parent = transform;
            missiles.Add(new_missile);
            reqMissile = new_missile;
        }
        reqMissile.GetComponent<missile>().parent_tank = parent_tank;
        reqMissile.SetActive(true);
        reqMissile.GetComponent<missile>().Init_Missile(aimSprite);
        return reqMissile;
    }
}
