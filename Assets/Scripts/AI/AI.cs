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

    [SerializeField] protected float _moveSpeed = 5f;
    [SerializeField] protected float _attackRange = 2f;
    [SerializeField] protected float _goodRange;

    protected bool _isAttack = true;

    protected Transform _modelTs;
    private Transform _stage;

    public bool IsKnockback = false;
    public bool IsPlayer = false;
    private Camera _mainCam;

    public override void Initialize() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _healthSystem = GetComponentInChildren<HealthSystem>();
        _healthSystem.Init();
        _weaponAnim = GetComponentInChildren<Animator>();

        _modelTs = transform.Find("Model");
        _stage = transform.parent;

        _mainCam = Camera.main;
    }

    public override void OnEpisodeBegin() {
        // 테스트용
        //transform.position = _stage.transform.position + new Vector3(Random.Range(-20, 22), Random.Range(-18, 8), 0);
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

        if (IsKnockback == false) {
            Vector2 movement = new Vector2(x, y).normalized;
            _rigidbody.velocity = movement * _moveSpeed;
        }

        Vector2 direction = Vector2.zero;
        float angle = 0f;
        if (IsPlayer) {
            return;
        }

        _target = GetClosestTarget();
        if (_target == null) {
            AddReward(-0.1f);
            return;
        }

        direction = (_target.transform.position - transform.position).normalized;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _modelTs.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _attackRange);
        foreach (Collider2D collider in colliders) {
            if (collider.gameObject == _target) {
                Attack(_target);
                break;
            }
        }


        //float distanceToTarget = Vector2.Distance(transform.position, _target.transform.position);
        //// 근거리 훈련
        //if (distanceToTarget < 1f) {
        //    AddReward(2f);
        //    EndEpisode();
        //    return;
        //}

        // 원거리 훈련
        //float abs = Mathf.Abs(distanceToTarget - _goodRange);
        //if (abs < 0.5f) {
        //    AddReward(50f);
        //    //EndEpisode();
        //}
        //else {
        //    AddReward(-abs * 0.01f);
        //}
    }

    private void Update() {
        if (!IsPlayer)
            return;

        Vector2 direction = Vector2.zero;
        float angle = 0f;

        Vector2 mousePosition = _mainCam.ScreenToWorldPoint(Input.mousePosition);
        direction = (mousePosition - (Vector2)transform.position).normalized;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _modelTs.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);


        if (Input.GetMouseButtonDown(0)) {
            CameraManager.Instance.ShakeCamera(0.1f, 0.3f);
            Attack(mousePosition);
        }

    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        actionsOut.ContinuousActions.Array[0] = Input.GetAxisRaw("Horizontal");
        actionsOut.ContinuousActions.Array[1] = Input.GetAxisRaw("Vertical");
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

    protected virtual void Attack(Vector2 pos) {
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

    protected IEnumerator AttackDelay(float delay) {
        yield return new WaitForSeconds(delay);
        _isAttack = true;
    }
}
