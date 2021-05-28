using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
// 테스트
// 정우 테스트

public class TankAgent : Agent
{    
    public GameObject ms_obj;
    public GameControl gc;
    public bool check;
    GameObject player_obj;
    public float position;
    public int wind;
    public GameObject ground_obj;
    public GameObject enemy_obj;
    public int curHealthOfEnemy;
    public int curHealthOfPlayer;
    public ObjManager ob;
    public missile ms;
    public float dist;

    [HideInInspector]
    public Ground gr;
    [HideInInspector]
    public playerAiming enemy;
    [HideInInspector]
    public playerAiming player;

    EnvironmentParameters m_ResetParams;

    public override void Initialize()
    {
        player_obj = this.gameObject;
        gr = ground_obj.GetComponent<Ground>();
        player = this.GetComponent<playerAiming>();
        enemy = enemy_obj.GetComponent<playerAiming>();

        m_ResetParams = Academy.Instance.EnvironmentParameters;
        

        SetResetParameters();
        
    }

    // 새로운 에피소드때 설정할 환경
    public override void OnEpisodeBegin()
    {
        if (player.curHealth != 100)
        {
            player.curHealth = 100;
            //player.curPower = 0;
            //player.curAngle = 0;
            enemy.curHealth = 100;
            //enemy.curPower = 0;
            //enemy.curAngle = 0;
            curHealthOfPlayer = player.curHealth;
            curHealthOfEnemy = enemy.curHealth;
        }
        if (enemy.curHealth != 100)
        {
            player.curHealth = 100;
            //player.curPower = 0;
            //player.curAngle = 0;
            enemy.curHealth = 100;
            //enemy.curPower = 0;
            //enemy.curAngle = 0;
            curHealthOfPlayer = player.curHealth;
            curHealthOfEnemy = enemy.curHealth;
        }

        check = false;

        //player.transform.localPosition = new Vector3(Random.Range(-17f,17f), -5.4f, 0);
        //enemy.transform.localPosition = new Vector3(Random.Range(-17f,17f), -5.4f, 0);
        player.transform.localPosition = new Vector3(Random.Range(-29f,29f), -5.4f, 0);
        enemy.transform.localPosition = new Vector3(Random.Range(-29f,29f), -5.4f, 0);

        //gr.resetTerrain();  // 일단 주석
        SetResetParameters();

        RequestDecision();
    }

    // 환경 관찰
    public override void CollectObservations(VectorSensor sensor)
    {   
    
        // 내 위치, 각도, 파워 보내기
        sensor.AddObservation(position);
        sensor.AddObservation(wind);
        // 적 위치 보내기
    }
    
    public void FixedUpdate(){

        if (check == true){

            if (player.getMissileCount() == 0 && gc.SetPlayer() == player_obj){
                if ( player.curHealth <= 0){
                    AddReward(-100f);
                    //Debug.Log("die");
                    //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
                    EndEpisode();
                }
                else if ( enemy.curHealth <= 0){
                    AddReward(100f);
                    Debug.Log("kill");
                    //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
                    EndEpisode();
                }
                
                else if (enemy.curHealth < curHealthOfEnemy)
                {
                    AddReward(15f);
                    Debug.Log("hit");
                    if((player.curAngle-90) * position > 0)
                        Debug.Log("!!!!!!!!!!!!!!!!!!!!!what the!!!!!!!!!!!!!!!!!!!!!!!!!");
                }
                else if (player.curHealth < curHealthOfPlayer)
                {
                    AddReward(-15f);
                    //Debug.Log("hurt");
                }
                else{
                    if(player.missile){
                        dist =  Mathf.Abs(player.missile.GetComponent<missile>().endPosition.x - enemy.transform.localPosition.x);
                        //Debug.Log(player.missile.GetComponent<missile>().endPosition.x+" "+enemy.transform.localPosition.x+" "+dist);
                        if(dist < 1){
                                AddReward(9f);
                        }
                        else if ( dist < 3 ){
                                AddReward(9/dist);
                                //Debug.Log(dist);
                        }
                        else
                                AddReward(-1f);
                    }
                    //Debug.Log("not hit");
                }
                Debug.Log(player.curPower +" "+ player.curAngle+" "+position+" "+wind+" "+curHealthOfEnemy+" "+enemy.curHealth);
                

                gc.WindChange();
                position = enemy.transform.localPosition.x-player.transform.localPosition.x;
                wind = gc.pow;
                RequestDecision();
            //Academy.Instance.EnvironmentStep();   
        }
         
    
        }
        
    }



    // 액션과 보상
   public override void OnActionReceived(ActionBuffers actionBuffers)
    {   
        check = true;
        /*if(check == false){   
            check = true;
            // power 는 0~100 사이의 정수
            player.curPower = Mathf.FloorToInt(100*Mathf.Clamp(actionBuffers.ContinuousActions[0],0f,1f));
            // angle 은 0~180 사이의 정수, 나중에 아래를 향해서 쏴야할 경우 181~360으로 변경해야할 수도 있음
            player.curAngle = Mathf.FloorToInt(180*Mathf.Clamp(actionBuffers.ContinuousActions[1],0f,1f));
            
            
            player.Shooting();
        }
        
        else if(check == true){
            if ( player.curHealth <= 0){
                AddReward(-30000f);
                //Debug.Log("die");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
                EndEpisode();
            }
            else if ( enemy.curHealth <= 0){
                AddReward(30000f);
                Debug.Log("kill");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
                EndEpisode();
            }
            else if (player.curHealth < curHealthOfPlayer)
            {
                AddReward(-1000f);
                //Debug.Log("hurt");
            }
            else if (enemy.curHealth < curHealthOfEnemy)
            {
                AddReward(10000f);
                Debug.Log("hit");
            }
            else{
                AddReward(-1000f);
                //Debug.Log("not hit");
            }
            check = false;
            Debug.Log(player.curPower +" "+ player.curAngle+" "+position+" "+wind);
            //Debug.Log(curHealthOfEnemy+" "+enemy.curHealth);
            Debug.Log(enemy.curHealth);
        }*/
        

        
        //Debug.Log(enemy.curHealth);
        
        
        
        // power 는 0~100 사이의 정수
        player.curPower = Mathf.FloorToInt(50*(actionBuffers.ContinuousActions[0]+1));
        // angle 은 0~180 사이의 정수, 나중에 아래를 향해서 쏴야할 경우 181~360으로 변경해야할 수도 있음
        player.curAngle = Mathf.FloorToInt(60*(actionBuffers.ContinuousActions[1]+1.5f));


        curHealthOfEnemy = enemy.curHealth;
        curHealthOfPlayer = player.curHealth;
        player.Shooting();
        //AddReward(100f);
        /*
        playerAiming.missileCount++;
        missile = MissileInfoSetting(ObjManager.Call().GetObject("missile"));
        FindObjectOfType<AudioManager>().Play("fire");
        player.curHealth += 10;
        */
        
        
        
        // AddReward(-0.1f);
    }


    public void SetTank()
    {
        //Set the attributes of the ball by fetching the information from the academy
        var scale = m_ResetParams.GetWithDefault("scale", 1.0f);
        player_obj.transform.localScale = new Vector3(scale, scale, scale);
    }
    
    public void SetResetParameters()
    {
        SetTank();
    }
}