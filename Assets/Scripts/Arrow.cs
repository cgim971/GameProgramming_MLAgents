using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {
    public float DamageAmount;

    private AI _ai;
    private Rigidbody2D _rigidbody;
    [SerializeField] private float _speed;

    public void Init(AI ai, Vector2 pos, Vector2 dir) {
        _ai = ai;
        _rigidbody = GetComponent<Rigidbody2D>();
        transform.position = pos;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        _rigidbody.velocity = dir * _speed;

        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        IDamage iDamage = other.GetComponent<IDamage>();
        if (iDamage != null) {
            if (_ai.GetComponent<IDamage>() == iDamage)
                return;

            iDamage.Damage(DamageAmount);

            AI ai = other.GetComponent<AI>();
            ai?.SetReward(-1f);

            Destroy(this.gameObject);
        }
    }
}
