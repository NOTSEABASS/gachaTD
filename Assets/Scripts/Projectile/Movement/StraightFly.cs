using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightFly : MonoBehaviour {
  public float speed;

  void Update() {
    transform.Translate(speed * Vector3.forward * Time.deltaTime, Space.Self);
  }
}
