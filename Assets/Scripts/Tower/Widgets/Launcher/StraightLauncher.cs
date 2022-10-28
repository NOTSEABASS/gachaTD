using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[System.Serializable]
public class StraightLauncher {
  [SerializeField]
  private PoolGetter<PoolObject> bulletGetter;
  [SerializeField]
  private Transform launchPoint;

  public void Launch(LaunchParam param) {
    if (param.target == null) {
      return;
    }
    if (bulletGetter.TryGet(out var bullet)) {
      bullet.transform.position = launchPoint.position;
      bullet.transform.LookAt(param.target.transform);
    }
  }
}
