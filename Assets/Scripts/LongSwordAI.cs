using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class LongSwordAI : AI {
    protected override void Attack(GameObject target) {
        if (!IsAttack())
            return;

        base.Attack(target);

        StartCoroutine(AttackDelay(0.1f, 0.3f));
    }
}
