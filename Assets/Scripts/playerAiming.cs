﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class playerAiming : MonoBehaviour
{
    public GameObject canvas_for_timer;
    public GameObject missile_prefab;
    public GameObject missile;
    public ObjManager ob; // pool manage 하기 위해 - 박상연

    public GameObject healthbar_obj;
    Healthbar healthbar;
    public GameObject playing_arrow;
    public GameControl gc;
    Animator animator;
    Rigidbody2D rb2d;

    [HideInInspector]
    public float speed = 5f;
    [HideInInspector]
    public int MinPower = 0;
    [HideInInspector]
    public int MaxPower = 100;
    public int curPower;
    public int MaxHealth = 100;
    [HideInInspector]
    public int curHealth = 100;


    public int curAngle = 0;
    [HideInInspector]
    public int minAngle = -20;
    [HideInInspector]
    public int maxAngle = 90;

    private float holdDownStartTime;

    int leftcount = 0;
    int rightcount = 0;

    public static int missileCount;
    bool isShootButtonDown = false;
    bool isPowerCharging = false;
    public bool isPlaying = false;
    public int isReflected = 1; // bool 에서 int로 바꿈

    public SpriteRenderer aimSprite;
    public Text powerTxt;
    public Text angleTxt;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();        
        curHealth = MaxHealth;
        healthbar = healthbar_obj.GetComponent<Healthbar>();
        healthbar.SetMaxHealth(curHealth);
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        //curPower = 0;
        //playing_arrow.SetActive(false);
        missileCount = 0;
        isReflected = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
        healthbar_obj.transform.localScale = new Vector3(isReflected*0.6f, healthbar_obj.transform.localScale.y, healthbar_obj.transform.localScale.z);
        transform.localScale = new Vector3(isReflected*1f, transform.localScale.y, transform.localScale.z);
        aimSprite.transform.rotation = Quaternion.Euler(0, 0, isReflected*(float)curAngle);
        //aimSprite.
        
        if (isPlaying)
        {
            playing_arrow.SetActive(true);

            if (Input.GetKeyDown(KeyCode.W))
            {
                rb2d.velocity = Vector2.up * 5f;
            }
            if (Input.GetKey(KeyCode.A))
            {
                isReflected = -1;
                animator.SetBool("isMoving", true);
                //rb2d.AddForce(new Vector2(-5f, 0f));
                transform.Translate(Vector3.left * Time.deltaTime * speed);
            }
            else if (Input.GetKey(KeyCode.D))
            {                
                isReflected = 1;
                animator.SetBool("isMoving", true);
                //rb2d.AddForce(new Vector2(5f, 0f));
                transform.Translate(Vector3.right * Time.deltaTime * speed);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (leftcount >= 3)
                {                    
                    if (curAngle < maxAngle)
                        curAngle++;
                    leftcount = 0;
                }
                else
                    leftcount++;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                if (rightcount >= 3)
                {                    
                    if (curAngle > minAngle)
                        curAngle--;
                    rightcount = 0;
                }
                else
                    rightcount++;
            }
            if (Input.GetKeyDown(KeyCode.Space) && missileCount == 0)
            {
                isPowerCharging = true;
                holdDownStartTime = Time.time;
            }
            if (Input.GetKeyUp(KeyCode.Space) && missileCount == 0)
            {
                Shooting();
                isPowerCharging = false;
            }
            if(isPowerCharging)
                curPower = (int)CalculateHoldPower(Time.time - holdDownStartTime);
            UpdateAim();
            healthbar.SetHealth(curHealth);
            if (curHealth <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
            }
        }
        else
        {
            playing_arrow.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("imhere");
        if (!col.GetComponent<CircleCollider2D>())
            return;
        if (col.CompareTag("Missile")){
            if(this.tag == "Player"){
                Debug.Log("phurt");
                //this.transform.GetComponent<TankAgent>().AddReward(-1f);
            }
            if(this.tag == "Enemy"){
                Debug.Log("phit");
                //this.transform.GetComponent<TankAgent>().AddReward(10f);
            }
            TakeDamage(10);
        }
    }

    

    public void TurnOver()
    {
        gc.ChangePlayer();
    }

    public int getMissileCount(){
        return missileCount;
    }

    void UpdateAim()
    {
        //aimSprite.transform.position = new Vector3(Mathf.Cos(curAngle*Mathf.Deg2Rad) * 0.5f + transform.position.x, Mathf.Sin(curAngle * Mathf.Deg2Rad) * 0.5f + transform.position.y, aimSprite.transform.position.z);
        //aimSprite.transform.localScale = new Vector3((float)curPower / 70 + 0.1f, aimSprite.transform.localScale.y, aimSprite.transform.localScale.z);
        
        aimSprite.transform.rotation = Quaternion.Euler(0, 0, (float)curAngle*isReflected);
        
        
        powerTxt.text = "Power: " + curPower;
        angleTxt.text = "Angle: " + curAngle;
    }

    public void Shooting()
    {
        if (missileCount == 0)
        {
            missileCount++;
            //GameObject temp_missile = Instantiate(missile_prefab, aimSprite.transform.position, aimSprite.transform.rotation);
            missile = MissileInfoSetting(ObjManager.Call().GetObject("missile"));
            /*   // pool 을 이용하기 위해 아래 함수 추가 후 주석처리
            temp_missile.SetActive(true);
            CameraFollow.target = temp_missile.transform;
            temp_missile.GetComponent<Rigidbody2D>().velocity = aimSprite.transform.right * curPower / 4.5f;
            temp_missile.GetComponent<Rigidbody2D>().AddForce(Vector3.forward * curPower / 5, ForceMode2D.Impulse);
            */
            FindObjectOfType<AudioManager>().Play("fire");
            //curPower = 0; // curPower가 0으로 보이는 버그를 수정하기위해 주석처리함 -박상연
            //isPlaying = false; // 다 내가 쏘기 위해
            canvas_for_timer.GetComponent<CountDownTimer>().TimeflowStop();
            //curHealth += 10;
            //return temp_missile;
        }
        //return null;
    }

    /*public GameObject Shooting()
    {
        if (missileCount == 0)
        {
            missileCount++;
            Vector3 vec;
            GameObject temp_missile;
            
            vec = new Vector3(aimSprite.transform.position.x + isReflected* 1.75f * Mathf.Cos(curAngle*Mathf.Deg2Rad), aimSprite.transform.position.y + 1.75f * Mathf.Sin(curAngle*Mathf.Deg2Rad), aimSprite.transform.position.z);
            temp_missile = Instantiate(missile_prefab, vec, aimSprite.transform.rotation);
            CameraFollow.target = temp_missile.transform;
            temp_missile.GetComponent<Rigidbody2D>().velocity = aimSprite.transform.right * curPower * isReflected / 4.5f;                
            

            temp_missile.GetComponent<Rigidbody2D>().AddForce(Vector3.forward * curPower / 5, ForceMode2D.Impulse);

            return temp_missile;
        }
        return null;
    }*/
    public GameObject MissileInfoSetting(GameObject _Missile){
        if(_Missile == null) { return _Missile;}
        Vector3 vec;

        UpdateAim();
        _Missile.transform.position = new Vector3(aimSprite.transform.position.x + isReflected* 1.75f * Mathf.Cos(curAngle*Mathf.Deg2Rad), aimSprite.transform.position.y + 1.75f * Mathf.Sin(curAngle*Mathf.Deg2Rad), aimSprite.transform.position.z);
        //_Missile.transform.Translate(new Vector3(1.2f*Mathf.Cos(curAngle*Mathf.PI/180f),1.2f*Mathf.Sin(curAngle*Mathf.PI/180f)),0);
        
        _Missile.transform.rotation = aimSprite.transform.rotation;
        _Missile.SetActive(true);
        _Missile.GetComponent<missile>().explosionArea.gameObject.SetActive(true);
        CameraFollow.target = _Missile.transform;
        _Missile.GetComponent<Rigidbody2D>().velocity = aimSprite.transform.right * curPower * isReflected / 4.5f;
        _Missile.GetComponent<Rigidbody2D>().AddForce(Vector3.forward * curPower / 5, ForceMode2D.Impulse);
        return _Missile;
    }


    public void TakeDamage(int damage)
    {
        curHealth -= damage;
        if (curHealth <= 0)
            curHealth = 0;
        healthbar.SetHealth(curHealth);        
    }
    private float CalculateHoldPower(float holdTime)
    {
        float maxHoldTime = 1.0f;
        float holdTimeNormalized = Mathf.Clamp01(holdTime / maxHoldTime);
        float power = holdTimeNormalized * MaxPower;
        return power;
    }
}
