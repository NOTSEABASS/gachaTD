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

    if (Input.GetKeyDown(KeyCode.Y)) {
      ReleaseToPool();
    }
  }

  public override void OnCreate() {
    Debug.Log("Zombie init");
  }

  public override void OnRelease() {
    Debug.Log("Zombie release");
  }

  public override void OnGet() {
    transform.position = Vector3.zero;
    print("Zombie get");
  }
}
