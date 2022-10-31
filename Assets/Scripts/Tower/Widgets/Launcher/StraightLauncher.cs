using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[System.Serializable]
public class StraightLauncher {
  [SerializeField]
  private PoolGetter<ProjectilePoolObject> bulletGetter;
  [SerializeField]
  private Transform launchPoint;

  public void Launch(LaunchParam param) {
    if (param.target == null) {
      return;
    }
    if (bulletGetter.TryGet(out var projectile)) {
      projectile.transform.position = launchPoint.position;
      projectile.transform.LookAt(param.target.transform);
      if (param.projectilePlugin != null) {
        projectile.SetPlugin(param.projectilePlugin);
      }
    }
  }
}
