using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class ZombieEnemyProxy : EnemyProxyBase {
    private float speed = 1.0f;
    private Transform GetNearbyTarget() {
        //todo: return nearest tower transform
        return PoolManager.Instance.transform;
    }

    private void MoveToTarget(Transform target) {
        var dir = (target.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
    }

    public override void OnUpdate() {
        MoveToTarget(GetNearbyTarget());
    }

    public ZombieEnemyProxy(Transform transform) : base(transform) { }
}
