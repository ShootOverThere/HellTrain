using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missile : MonoBehaviour
{
    public GameObject explosionArea;
    public Rigidbody2D rb2d;
    public CameraFollow cm;
    public Vector2 endPosition;
    bool hasHit = false;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        //ObjManager.Call().SetObject("missile");
        //Destroy(gameObject, 5f);
        endPosition = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasHit == false)
        {
            float angle = Mathf.Atan2(rb2d.velocity.y, rb2d.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        if(rb2d.transform.position.y <= -30)
            OnDestroy();
    }

    void OnDestroy()
    {   gameObject.SetActive(false);
        explosionArea.gameObject.SetActive(false);
        endPosition = new Vector2(rb2d.transform.position.x, rb2d.transform.position.y);
        //CameraFollow.target = GameObject.FindWithTag("Player").transform;
        GameObject go = GameObject.Find("Main Camera");
        if (go){
            go.GetComponent<CameraFollow>().CameraReset();
        }
        Reset();
        playerAiming.missileCount--;
        
        
        
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Ground"))
        {
            //explosionArea.gameObject.SetActive(true);
            //Destroy(this.gameObject, 0.02f);
            
            OnDestroy();
            
            //gameObject.SetActive(false); // 박상연
        }
        if(other.gameObject.CompareTag("Player"))
        {// 여긴 아예 일어나지가 않아 진짜 화가 잔뜩나네
            
            
        }
        if(other.gameObject.CompareTag("Enemy")){
            
        }
        //hasHit = true;  // 실험상 주석 - 박상연
        //rb2d.isKinematic = true; // 실험상 주석 - 박상연
        
    }


    void Reset(){
        hasHit = false;
        //rb2d.isKinematic = false;
        rb2d.transform.localPosition = Vector2.zero;
        rb2d.velocity = Vector2.zero;
    }
    
    /*
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Missile Enter Tag: " + other.gameObject.tag);
        if (other.gameObject.tag == "Player")
        {
            playerAiming pA;
            pA = other.gameObject.GetComponent<playerAiming>();
            pA.TakeDamage(10);
        }
        if (other.gameObject.tag == "Enemy")
        {
            Enemy en;
            en = other.gameObject.GetComponent<Enemy>();
            en.TakeDamage(10);
        }
    }*/
}
