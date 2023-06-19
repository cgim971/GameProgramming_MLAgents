using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour {


    public float Health => _health;
    public float MaxHealth => _maxHealth;

    private float _health;
    [SerializeField] private float _maxHealth;

    public void Init() {
        _health = MaxHealth;
    }

    public void Damage(float damage) {
        _health = Mathf.Clamp(_health - damage, 0, _maxHealth);

        if (_health <= 0) {
            Die();
        }
    }

    public void Die() {
        // Die
        Debug.Log("Die");
    }
}
