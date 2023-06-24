using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class BowAI : AI {
    [SerializeField] private Arrow _arrow;

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
        float abs = Mathf.Abs(distanceToTarget - _goodRange);
        if (abs < 0.5f) {
            AddReward(50f);
            // 이동 훈련할 때만 사용
            // EndEpisode();
        }
        else {
            AddReward(-abs * 0.01f);
        }
    }


    protected override void Attack(GameObject target) {
        if (!IsAttack())
            return;

        base.Attack(target);

        // 화살 쏘기
        Arrow newArrow = Instantiate(_arrow, null);

        Vector2 dir = (target.transform.position - transform.position).normalized;
        newArrow.Init(this, transform.position, dir);

        StartCoroutine(AttackDelay(1f, 1.5f));
    }
}
