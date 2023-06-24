using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeAI : AI {
    protected override void Attack(GameObject target) {
        if (!IsAttack())
            return;

        base.Attack(target);

        StartCoroutine(AttackDelay(0.8f, 1.4f));
    }
}
