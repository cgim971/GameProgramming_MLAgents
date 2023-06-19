using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class LongSwordAI : AI {
    protected override void Attack(GameObject target) {
        SetReward(3f);
        //target.GetComponent<HealthSystem>().Damage(DamageAmount);
        //target.GetComponent<AI>().SetReward(-1f);
    }
}
