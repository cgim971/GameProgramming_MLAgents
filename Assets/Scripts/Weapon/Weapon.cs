using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    public float DamageAmount;
    protected AI _ai;
    public GameObject Blood;

    protected void Start() {
        _ai = GetComponentInParent<AI>();
    }

    protected virtual void OnTriggerEnter2D(Collider2D other) {
        IDamage iDamage = other.GetComponent<IDamage>();
        if (iDamage != null) {
            if (_ai?.GetComponent<IDamage>() == iDamage)
                return;

            iDamage.Damage(DamageAmount);

            Effect(other.gameObject);

            AI ai = other.GetComponent<AI>();
            ai?.SetReward(-1f);

            // Effect
            BloodEffect(other);
        }
    }

    public void BloodEffect(Collider2D obj) {
        GameObject effect = Instantiate(Blood, obj.transform);
        Vector2 closestPoint = obj.ClosestPoint(transform.position);
        effect.transform.position = closestPoint;
        Destroy(effect, 1.1f);
    }

    public virtual void Effect(GameObject obj) { }
}
