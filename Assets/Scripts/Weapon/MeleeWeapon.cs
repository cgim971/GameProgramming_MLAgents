using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour {
    public float DamageAmount;
    private AI _ai;

    private void Start() {
        _ai = GetComponentInParent<AI>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        IDamage iDamage = other.GetComponent<IDamage>();
        if (iDamage != null) {
            if (_ai.GetComponent<IDamage>() == iDamage)
                return;

            iDamage.Damage(DamageAmount);

            AI ai = other.GetComponent<AI>();
            ai?.SetReward(-1f);
        }
    }
}
