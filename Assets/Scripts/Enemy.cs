using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Healthbar healthbar;
    public int MaxHealth = 100;
    public int curHealth;

    // Start is called before the first frame update
    void Start()
    {
        curHealth = MaxHealth;
        healthbar.SetMaxHealth(curHealth);
    }

    // Update is called once per frame
    void Update()
    {
        healthbar.SetHealth(curHealth);
        if(curHealth <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.CompareTag("Missile"))
        {
            TakeDamage(10);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.GetComponent<CircleCollider2D>())
            return;
        if (col.CompareTag("Missile"))
            TakeDamage(10);
    }

    public void TakeDamage(int damage)
    {
        if (curHealth - damage <= 0)
            curHealth = 0;
        else
            curHealth -= damage;
        healthbar.SetHealth(curHealth);
    }
}
