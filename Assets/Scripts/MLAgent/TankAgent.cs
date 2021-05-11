using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class TankAgent : Agent
{
    GameObject player_obj;
    public GameObject enemy_obj;

    [HideInInspector]
    public playerAiming enemy;
    [HideInInspector]
    public playerAiming player;

    EnvironmentParameters m_ResetParams;

    // Start is called before the first frame update

    public override void Initialize()
    {
        player_obj = this.gameObject;
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

        player.transform.localPosition = new Vector3(-12f, -5.4f, 0);
        enemy.transform.localPosition = new Vector3(11.42f, -5.4f, 0);

        SetResetParameters();
    }

    // 환경 관찰
    public override void CollectObservations(VectorSensor sensor)
    {
        // 내 위치, 각도, 파워 보내기
        sensor.AddObservation(player.transform.localPosition);
        sensor.AddObservation(player.curAngle);
        sensor.AddObservation(player.curPower);
        sensor.AddObservation(player.curHealth);
        // 적 위치 보내기
        sensor.AddObservation(enemy_obj.transform.localPosition);
        sensor.AddObservation(enemy.curHealth);
    }

    // 액션과 보상
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        /*
        int power = Mathf.FloorToInt(Mathf.Clamp(actionBuffers.ContinuousActions[0], 0, 100));
        int angle = Mathf.FloorToInt(Mathf.Clamp(actionBuffers.ContinuousActions[1], 0, 360));
        int shootAction = Mathf.FloorToInt(Mathf.Clamp(actionBuffers.ContinuousActions[2], 0, 1));
        */

        var power = Mathf.Clamp(actionBuffers.ContinuousActions[0], 0, 100);
        var angle = Mathf.Clamp(actionBuffers.ContinuousActions[1], 0, 360);
        var shootAction = (int)Mathf.Clamp(actionBuffers.ContinuousActions[2], 0, 1);
        bool isShoot = true;

        gameObject.transform.Rotate(new Vector3(0, 0, 1), power);
        if (shootAction == 0)
        {
            isShoot = false;
        }
        else
        {
            isShoot = true;
        }
        
        // 액션 조정
        //player.curPower = (int)actionBuffers.ContinuousActions[0];
        //player.curAngle = (int)actionBuffers.ContinuousActions[1];
        player.curPower = Mathf.FloorToInt(power);
        player.curAngle = Mathf.FloorToInt(angle);

        if(isShoot == true)
        {
            player.Shooting();
            SetReward(1f);
        }
        //player.GetComponent<Rigidbody2D>().AddForce(new Vector2(5* vectorActions[0], 5 * vectorActions[1]));

        // 보상
        if(enemy.curHealth != 100)
        {
            SetReward(10.0f);
            EndEpisode();
        }

        if(player.curHealth != 100)
        {
            SetReward(-10f);
            EndEpisode();
        }

        SetReward(-0.05f);
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
