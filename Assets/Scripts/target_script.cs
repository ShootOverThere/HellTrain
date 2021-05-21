using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class target_script : MonoBehaviour
{
    public int curHealth = 100;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.GetComponent<CircleCollider2D>())
            return;
        if (col.CompareTag("Missile"))
        {
            int damage = 10;
            curHealth -= damage;
            if (curHealth <= 0)
                curHealth = 0;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
