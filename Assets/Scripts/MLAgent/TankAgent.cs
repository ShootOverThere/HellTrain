using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System.Threading;
// 테스트
// 정우 테스트

public class TankAgent : Agent
{
    public int MinPower = 0;
    public int MaxPower = 100;
    public int MaxHealth = 100;
    public int curHealth = 100;
    public int minAngle = -360;
    public int maxAngle = 360;

    public int missileCount = 1;
    public SpriteRenderer aimSprite;

    public int param_power = 0;
    public int param_angle = 0;


    public GameObject missile_prefab;

    public Healthbar healthbar;
    public GameObject playing_arrow;
    public GameControl gc;
    Rigidbody2D rb2d;

    public GameObject ground_obj;
    public target_script enemy_agent;
    public GameObject ms_obj;
    public Ground gr;

    GameObject missile_obj;

    EnvironmentParameters m_ResetParams;

    public override void Initialize()
    {
        curHealth = MaxHealth;
        enemy_agent.curHealth = MaxHealth;

        ObjectManager.parent_tank = this;

        healthbar.SetMaxHealth(curHealth);
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        gr = ground_obj.GetComponent<Ground>();
        m_ResetParams = Academy.Instance.EnvironmentParameters;
        RequestDecision();
        SetResetParameters();
    }

    // 새로운 에피소드때 설정할 환경
    public override void OnEpisodeBegin()
    {
        curHealth = MaxHealth;
        enemy_agent.curHealth = MaxHealth;

        gr.resetTerrain();
        SetResetParameters();
    }

    public void missile_destroyed()
    {
        ObjectManager.missile_count = 0;

        if (enemy_agent.curHealth < MaxHealth)
        {
            SetReward(1f);
            EndEpisode();
        }
        else if (this.curHealth < MaxHealth)
        {
            AddReward(-1f);
            EndEpisode();
        }
        else
            AddReward(-0.1f);
        RequestDecision();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(enemy_agent.transform.position.x - this.transform.position.x);
    }

    public void Update()
    {
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        if (ObjectManager.missile_count == 0)
        {
            int power = actionBuffers.DiscreteActions[0];
            int angle = actionBuffers.DiscreteActions[1];
            Shooting(power, angle);
        }
    }

    public void SetTank()
    {
        //Set the attributes of the ball by fetching the information from the academy
        var scale = m_ResetParams.GetWithDefault("scale", 1.0f);
    }
    
    public void SetResetParameters()
    {
        SetTank();
    }

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
            healthbar.SetHealth(curHealth);
        }
    }

    void UpdateAim(int param_power, int param_angle)
    {
        aimSprite.transform.position = new Vector3(Mathf.Cos(param_angle * Mathf.Deg2Rad) * 0.5f + transform.position.x, Mathf.Sin(param_angle * Mathf.Deg2Rad) * 0.5f + transform.position.y, aimSprite.transform.position.z);
        aimSprite.transform.localScale = new Vector3((float)param_power / 70 + 0.1f, aimSprite.transform.localScale.y, aimSprite.transform.localScale.z);
        aimSprite.transform.rotation = Quaternion.Euler(0, 0, (float)param_angle);
    }

    public void Shooting(int param_power, int param_angle)
    {
        UpdateAim(param_power, param_angle);
        missile_obj = ObjectManager.instance.GetMissile(aimSprite);
        missile_obj.GetComponent<missile>().parent_tank = this;
        missile_obj.GetComponent<Rigidbody2D>().velocity = aimSprite.transform.right * param_power / 4.5f;
        missile_obj.GetComponent<Rigidbody2D>().AddForce(Vector3.forward * param_power / 5, ForceMode2D.Impulse);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.DiscreteActions;
        continuousActionsOut[0] = 25;
        continuousActionsOut[1] = 90;
    }

}
