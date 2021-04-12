using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class TankAgent : Agent
{
    public GameObject enemy_obj;

    [HideInInspector]
    public playerAiming enemy;
    [HideInInspector]
    public playerAiming player;

    // Start is called before the first frame update
    void Start()
    {
        player = this.GetComponent<playerAiming>();
        enemy = enemy_obj.GetComponent<playerAiming>();
    }

    public override void Initialize()
    {
        player = this.GetComponent<playerAiming>();
        enemy = enemy_obj.GetComponent<playerAiming>();
    }

    // 새로운 에피소드때 설정할 환경
    public override void OnEpisodeBegin()
    {
        if (player.curHealth != 100)
        {
            player.curHealth = 100;
            player.curPower = 0;
            player.curAngle = 0;
            //enemy.curHealth = 100;
        }
        enemy.curHealth = 100;
    }

    // 환경 관찰
    public override void CollectObservations(VectorSensor sensor)
    {
        // 내 위치, 각도, 파워 보내기
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(player.curAngle);
        sensor.AddObservation(player.curPower);
        // 적 위치 보내기
        sensor.AddObservation(enemy_obj.transform.localPosition);
    }

    // 액션과 보상
    public override void OnActionReceived(float[] vectorActions)
    {
        float power = vectorActions[0] * 1000f;
        float angle = vectorActions[1] * 1000f;
        // 액션 조정
        //player.curPower = (int)actionBuffers.ContinuousActions[0];
        //player.curAngle = (int)actionBuffers.ContinuousActions[1];
        player.curPower = (int)power;
        player.curAngle = (int)angle;
        player.GetComponent<Rigidbody2D>().AddForce(new Vector2(5* vectorActions[0], 5 * vectorActions[1]));

        // 보상
        if(enemy.curHealth != 100)
        {
            SetReward(1.0f);
            EndEpisode();
        }

        if(player.curHealth != 100)
        {
            SetReward(-1.0f);
            EndEpisode();
        }
    }
}
