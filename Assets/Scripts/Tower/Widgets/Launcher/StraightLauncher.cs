using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class StraightLauncher {
  [SerializeField]
  private PoolGetter<ProjectilePoolObject> bulletGetter;
  [SerializeField]
  private Transform launchPoint;
  [SerializeField]
  private bool zAiming;
  public void Launch(LaunchParam param) {
    if (param.target == null) {
      return;
    }
    if (bulletGetter.TryGet(out var projectile)) {
      projectile.transform.position = launchPoint.position;
      projectile.transform.LookAt(param.target.transform);
      if (!zAiming) {
        var forward = projectile.transform.forward;
        forward.y = 0;
        projectile.transform.forward = forward;

      }
      if (param.projectilePlugin != null) {
        projectile.SetPlugin(param.projectilePlugin);
      }
    }
  }
}
