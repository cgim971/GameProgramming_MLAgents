using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class BowAI : AI {
    [SerializeField] private Arrow _arrow;

    protected override void Attack(GameObject target) {
        if (!IsAttack())
            return;

        base.Attack(target);

        // È­»ì ½î±â
        Arrow newArrow = Instantiate(_arrow, null);

        Vector2 dir = (target.transform.position - transform.position).normalized;
        newArrow.Init(this, transform.position, dir);

        StartCoroutine(AttackDelay(0.6f, 1f));
    }

    protected override void Attack(Vector2 pos) {
        if (!IsAttack())
            return;

        base.Attack(pos);

        Arrow newArrow = Instantiate(_arrow, null);

        Vector2 dir = (pos - (Vector2)transform.position).normalized;
        newArrow.Init(this, transform.position, dir);

        StartCoroutine(AttackDelay(0.3f));
    }
}
