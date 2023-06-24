using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class AI : Agent, IDamage {
    // Target을 매니저로 넣기
    public List<GameObject> TargetList => _targetList;
    [SerializeField] protected List<GameObject> _targetList = new List<GameObject>();

    protected Rigidbody2D _rigidbody;
    protected HealthSystem _healthSystem;
    protected Animator _weaponAnim;
    protected GameObject _target;

    protected float _moveSpeed = 5f;
    [SerializeField] protected float _attackRange = 2f;
    [SerializeField] protected float _goodRange;

    protected bool _isAttack = true;

    protected Transform _modelTs;
    private Transform _stage;

    public override void Initialize() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _healthSystem = GetComponentInChildren<HealthSystem>();
        _healthSystem.Init();
        _weaponAnim = GetComponentInChildren<Animator>();

        _modelTs = transform.Find("Model");
        _stage = transform.parent;
    }

    public override void OnEpisodeBegin() {
        transform.position = (Vector2)_stage.transform.position + new Vector2(Random.Range(-10f, 10f), Random.Range(-5f, 5f));
    }

    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(_rigidbody.velocity);
        sensor.AddObservation(_target != null ? (Vector2)_target.transform.position : Vector2.zero);
    }

    public override void OnActionReceived(ActionBuffers actions) {
        var action = actions.ContinuousActions;

        float x = action[0];
        float y = action[1];

        Vector2 movement = new Vector2(x, y).normalized;
        _rigidbody.velocity = movement * _moveSpeed;

        _target = GetClosestTarget();
        if (_target == null) {
            AddReward(-0.1f);
            return;
        }

        Vector2 direction = _target.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _modelTs.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _attackRange);
        foreach (Collider2D collider in colliders) {
            if (collider.gameObject == _target) {
                Attack(_target);
                break;
            }
        }

        float distanceToTarget = Vector2.Distance(transform.position, _target.transform.position);
        if (distanceToTarget < 1.5f) {
            AddReward(2f);
            // 이동 훈련할 때만 사용
            // EndEpisode();
        }
    }

    protected bool IsAttack() {
        if (_weaponAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack") || !_isAttack)
            return false;

        return true;
    }

    protected virtual void Attack(GameObject target) {
        _isAttack = false;
        _weaponAnim.SetTrigger("Attack");
        SetReward(2f);
    }

    protected GameObject GetClosestTarget() {
        GameObject target = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject go in _targetList) {
            if (go == null)
                continue;

            float distance = Vector2.Distance(transform.position, go.transform.position);
            if (distance < closestDistance && !Physics2D.Linecast(transform.position, go.transform.position, LayerMask.GetMask("Obstacle"))) {
                closestDistance = distance;
                target = go;
            }
        }

        return target;
    }

    public void Damage(float damage) => _healthSystem.Damage(damage);

    protected IEnumerator AttackDelay(float min, float max) {
        yield return new WaitForSeconds(Random.Range(min, max));
        _isAttack = true;
    }
}
