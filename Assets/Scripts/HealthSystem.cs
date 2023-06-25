using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour {
    public float Health => _health;
    public float MaxHealth => _maxHealth;

    [SerializeField] private float _health;
    [SerializeField] private float _maxHealth;

    private AI _ai;
    private Transform _barTransform;

    public void Init() {
        _health = MaxHealth;
        _ai = transform.GetComponentInParent<AI>();
        _barTransform = transform.Find("bar");
    }

    public void Damage(float damage) {
        _health = Mathf.Clamp(_health - damage, 0, _maxHealth);

        if (_ai.IsPlayer)
            CameraManager.Instance.ShakeCamera(0.2f, 5f);

        UpdateBar();

        if (_health <= 0) {
            Die();
        }
    }

    public void Die() {
        // Die
        Debug.Log("Die");
        if (_ai != null) {
            StageManager.Instance.Die(_ai, _ai.IsPlayer);

            Destroy(_ai.gameObject);
        }
    }

    private void UpdateBar() {
        _barTransform.localScale = new Vector3(GetHealthAmountNormalized(), 0.2f, 1);
    }

    public float GetHealthAmountNormalized() => (float)Health / MaxHealth;
}
