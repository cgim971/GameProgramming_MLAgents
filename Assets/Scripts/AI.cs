using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.Animations;

public class AI : Agent {
    public List<GameObject> TargetList => _targetList;
    [SerializeField] protected List<GameObject> _targetList = new List<GameObject>();

    protected Rigidbody2D _rigidbody;
    [SerializeField] protected GameObject _target;

    protected float _moveSpeed = 5f;
    protected float _attackRange = 2f;
    public float DamageAmount;

    protected Transform _modelTs;

    public override void Initialize() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _modelTs = transform.Find("Model");
    }

    public override void OnEpisodeBegin() {
        transform.position = new Vector2(Random.Range(-10f, 10f), Random.Range(-5f, 5f));
    }

    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(_rigidbody.velocity);
    }

    public override void OnActionReceived(ActionBuffers actions) {
        var action = actions.ContinuousActions;

        float x = action[0];
        float y = action[1];

        Vector2 movement = new Vector2(x, y).normalized;
        _rigidbody.velocity = movement * _moveSpeed;

        _target = GetClosestTarget();
        if (_target == null) {
            SetReward(-0.1f);
            return;
        }

        Vector2 direction = _target.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _modelTs.transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _attackRange);
        foreach (Collider2D collider in colliders) {
            if (collider.gameObject == _target) {
                // Attack
                Attack(_target);
                break;
            }
        }

        float distanceToTarget = Vector2.Distance(transform.position, _target.transform.position);
        if (distanceToTarget < 1.5f) {
            SetReward(5f);
            EndEpisode();
        }
    }

    protected virtual void Attack(GameObject target) { }

    protected GameObject GetClosestTarget() {
        GameObject target = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject go in _targetList) {
            float distance = Vector2.Distance(transform.position, go.transform.position);
            if (distance < closestDistance && !Physics2D.Linecast(transform.position, go.transform.position, LayerMask.GetMask("Obstacle"))) {
                closestDistance = distance;
                target = go;
            }
        }

        return target;
    }
}
