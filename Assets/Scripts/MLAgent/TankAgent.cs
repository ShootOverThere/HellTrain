using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
// 희원 테스트
// 정우 테스트

public class TankAgent : Agent
{    
    GameObject player_obj;
    
    public GameObject ground_obj;
    public GameObject enemy_obj;
    public GameControl gc;

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
            player.curPower = 0;
            player.curAngle = 0;
            enemy.curHealth = 100;
            enemy.curPower = 0;
            enemy.curAngle = 0;
        }
        if (enemy.curHealth != 100)
        {
            player.curHealth = 100;
            player.curPower = 0;
            player.curAngle = 0;
            enemy.curHealth = 100;
            enemy.curPower = 0;
            enemy.curAngle = 0;
        }

        player.transform.localPosition = new Vector3(-8f, -5.4f, 0);
        enemy.transform.localPosition = new Vector3(8f, -5.4f, 0);

        gr.resetTerrain();
        SetResetParameters();
    }

    // 환경 관찰
    public override void CollectObservations(VectorSensor sensor)
    {   
    
        // 내 위치, 각도, 파워 보내기
        sensor.AddObservation(player.transform.localPosition.x);
        sensor.AddObservation(gc.wind_field.forceMagnitude);
        //sensor.AddObservation(player.curHealth);
        // 적 위치 보내기
        sensor.AddObservation(enemy_obj.transform.localPosition.x);
        //sensor.AddObservation(enemy.curHealth);
    }
    

    // 액션과 보상
   public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // power 는 0~100 사이의 정수
        int power = Mathf.FloorToInt(100*Mathf.Abs(actionBuffers.ContinuousActions[0]));
        // angle 은 0~180 사이의 정수, 나중에 아래를 향해서 쏴야할 경우 181~360으로 변경해야할 수도 있음
        int angle = Mathf.FloorToInt(180*Mathf.Abs(actionBuffers.ContinuousActions[1]));
        int shootAction = Mathf.FloorToInt(Mathf.Clamp(actionBuffers.ContinuousActions[2], 0, 1));
        
        /*bool isShoot = true;

        if (shootAction == 0)
        {
            isShoot = false;
        }
        else
        {
            isShoot = true;
        }*/
        
        // 액션 조정
        player.curPower = power;
        player.curAngle = angle;

        /*if(isShoot == true)
        {
            if(playerAiming.missileCount == 0)
                player.Shooting();
                SetReward(1f);
        }*/

        if(playerAiming.missileCount == 0){
                player.Shooting();
        }

        // 보상
        if (player.transform.position.y < -30f)
        {
            SetReward(-30f);
            EndEpisode();
        }
        if (enemy.transform.position.y < -30f)
        {
            SetReward(30f);
            EndEpisode();
        }
        if (enemy.curHealth != 100)
        {
            SetReward(100.0f);
            EndEpisode();
        }

        if(player.curHealth != 100)
        {
            SetReward(-10f);
            EndEpisode();
        }
        SetReward(-0.01f);
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