using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeWeapon : Weapon {
    public float KnockbackForce = 50f;

    public override void Effect(GameObject obj) {
        AI ai = obj.GetComponent<AI>();
        ai.IsKnockback = true;

        Rigidbody2D rigidbody = obj.GetComponent<Rigidbody2D>();
        Vector2 knockbackDirection = (obj.transform.position - _ai.transform.position).normalized;

        if (rigidbody != null) {
            rigidbody.velocity = Vector2.zero;
            rigidbody.AddForce(knockbackDirection * KnockbackForce, ForceMode2D.Impulse);
            StartCoroutine(SetIsKnockback(ai));
        }
    }

    IEnumerator SetIsKnockback(AI ai) {
        yield return new WaitForSeconds(0.5f);
        ai.IsKnockback = false;
    }
}
