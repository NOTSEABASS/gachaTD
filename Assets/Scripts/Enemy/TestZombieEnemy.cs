using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestZombieEnemy : PoolObject {
    private Transform target;

    public float speed = 1.0f;
    // Start is called before the first frame update
    void Start() {
        target = PoolManager.Instance.transform;
    }

    // Update is called once per frame
    void Update() {
        var dir = (target.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
    }

    public override void Init() {
        Debug.Log("Zombie Init");
    }

    public override void Reset() {
        Debug.Log("Zombie Reset");
    }
}
