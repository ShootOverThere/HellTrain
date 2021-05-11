using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class playerAiming : MonoBehaviour
{
    public GameObject canvas_for_timer;
    public GameObject missile_prefab;
    GameObject missile;
    public Healthbar healthbar;
    public GameObject playing_arrow;
    public GameControl gc;
    Rigidbody2D rb2d;

    public float speed = 5f;
    public int MinPower = 0;
    public int MaxPower = 100;
    public int curPower = 0;
    public int MaxHealth = 100;
    public int curHealth = 100;
    
    public int curAngle = 0;
    public int minAngle = -360;
    public int maxAngle = 360;

    private float holdDownStartTime;

    int leftcount = 0;
    int rightcount = 0;

    public static int missileCount = 0;
    bool isShootButtonDown = false;
    bool isPowerCharging = false;
    public bool isPlaying = false;

    public SpriteRenderer aimSprite;
    public Text powerTxt;
    public Text angleTxt;
    // Start is called before the first frame update
    void Start()
    {        
        curHealth = MaxHealth;
        healthbar.SetMaxHealth(curHealth);
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        curPower = 0;
        //playing_arrow.SetActive(false);
        missileCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            playing_arrow.SetActive(true);

            if (Input.GetKeyDown(KeyCode.T))
            {
                curAngle = 150;
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                rb2d.velocity = Vector2.up * 5f;
            }
            if (Input.GetKey(KeyCode.A))
            {
                //rb2d.AddForce(new Vector2(-5f, 0f));
                transform.Translate(Vector3.left * Time.deltaTime * speed);
            }
            if (Input.GetKey(KeyCode.D))
            {
                //rb2d.AddForce(new Vector2(5f, 0f));
                transform.Translate(Vector3.right * Time.deltaTime * speed);
            }

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (leftcount >= 5)
                {
                    curAngle++;
                    if (curAngle == maxAngle)
                        curAngle = 0;
                    leftcount = 0;
                }
                else
                    leftcount++;
            }
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (rightcount >= 5)
                {
                    curAngle--;
                    if (curAngle == minAngle)
                        curAngle = 0;
                    rightcount = 0;
                }
                else
                    rightcount++;
            }
            if (Input.GetKeyDown(KeyCode.Space) && missileCount == 0)
            {
                isPowerCharging = true;
                holdDownStartTime = Time.time;
                /*
                if (powerCount == 3)
                {
                    if (curPower < 100 && isPowerCharging)
                        curPower++;
                    else if (curPower == 100)
                    {
                        isPowerCharging = false;
                    }

                    if (curPower > 0 && !isPowerCharging)
                    {
                        curPower--;
                    }
                    else if (curPower == 0)
                    {
                        isPowerCharging = true;
                    }
                    powerCount = 0;
                }
                else
                    powerCount++;
                */
            }
            if (Input.GetKeyUp(KeyCode.Space) && missileCount == 0)
            {
                //float holdDownTime = Time.time - holdDownStartTime;
                //curPower = (int)CalculateHoldPower(holdDownTime);
                Shooting();
                //missileCount++;
                isPowerCharging = false;
                /*
                isShootButtonDown = false;
                Shooting();
                missileCount++;
                powerCount = 0;
                */
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
            aimSprite.transform.localScale = new Vector3(0, aimSprite.transform.localScale.y, aimSprite.transform.localScale.z);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.GetComponent<CircleCollider2D>())
            return;
        if (col.CompareTag("Missile"))
            TakeDamage(10);
    }

    public void TurnOver()
    {
        gc.ChangePlayer();
    }

    void UpdateAim()
    {
        aimSprite.transform.position = new Vector3(Mathf.Cos(curAngle*Mathf.Deg2Rad) * 0.5f + transform.position.x, Mathf.Sin(curAngle * Mathf.Deg2Rad) * 0.5f + transform.position.y, aimSprite.transform.position.z);
        aimSprite.transform.localScale = new Vector3((float)curPower / 70 + 0.1f, aimSprite.transform.localScale.y, aimSprite.transform.localScale.z);
        aimSprite.transform.rotation = Quaternion.Euler(0, 0, (float)curAngle);
        powerTxt.text = "Power: " + curPower;
        angleTxt.text = "Angle: " + curAngle;
    }

    public void Shooting()
    {
        missileCount++;
        GameObject missile = Instantiate(missile_prefab, aimSprite.transform.position, aimSprite.transform.rotation);
        CameraFollow.target = missile.transform;
        missile.GetComponent<Rigidbody2D>().velocity = aimSprite.transform.right * curPower / 4.5f;
        missile.GetComponent<Rigidbody2D>().AddForce(Vector3.forward * curPower / 5, ForceMode2D.Impulse);
        FindObjectOfType<AudioManager>().Play("fire");
        curPower = 0;
        isPlaying = false;
        canvas_for_timer.GetComponent<CountDownTimer>().TimeflowStop();
    }

    public void TakeDamage(int damage)
    {
        curHealth -= damage;
        if (curHealth <= 0)
            curHealth = 0;
        healthbar.SetHealth(curHealth);        
    }

    public void PowerUp()
    {
        if (curPower >= MinPower && curPower < MaxPower)
            curPower++;
    }

    public void PowerDown()
    {
        if (curPower > MinPower && curPower <= MaxPower)
            curPower--;
    }

    public void AngleUp()
    {
        curAngle++;
        if (curAngle == maxAngle)
            curAngle = 0;
    }

    public void AngleDown()
    {
        curAngle--;
        if (curAngle == minAngle)
            curAngle = 0;
    }

    private float CalculateHoldPower(float holdTime)
    {
        float maxHoldTime = 1.0f;
        float holdTimeNormalized = Mathf.Clamp01(holdTime / maxHoldTime);
        float power = holdTimeNormalized * MaxPower;
        return power;
    }
}
