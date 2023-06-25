using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAI : AI {
    [SerializeField] private Magic _magic;

    protected override void Attack(GameObject target) {
        if (!IsAttack())
            return;

        base.Attack(target);

        // È­»ì ½î±â
        Magic newArrow = Instantiate(_magic, null);

        Vector2 dir = (target.transform.position - transform.position).normalized;
        newArrow.Init(this, transform.position, dir);

        StartCoroutine(AttackDelay(0.8f, 1.3f));
    }

    protected override void Attack(Vector2 pos) {
        if (!IsAttack())
            return;

        base.Attack(pos);

        Magic newArrow = Instantiate(_magic, null);

        Vector2 dir = (pos - (Vector2)transform.position).normalized;
        newArrow.Init(this, transform.position, dir);

        StartCoroutine(AttackDelay(0.7f));
    }
}
