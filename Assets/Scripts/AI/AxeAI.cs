using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeAI : AI {
    protected override void Attack(GameObject target) {
        if (!IsAttack())
            return;

        base.Attack(target);

        StartCoroutine(AttackDelay(1.4f, 1.8f));
    }

    protected override void Attack(Vector2 pos) {
        if (!IsAttack())
            return;

        base.Attack(pos);

        StartCoroutine(AttackDelay(1f));
    }
}
