using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class RotationAiming {
  public bool zAiming;
  public Transform aimRoot;

  public void Aim(GameObject target) {
    aimRoot.LookAt(target.transform.position);
    if (!zAiming) {
      var forward = aimRoot.forward;
      forward.y = 0;
      aimRoot.forward = forward;
    }
  }
}
