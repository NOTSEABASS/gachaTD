using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StraightLauncher {
  [SerializeField]
  private GameObject bulletPrefab;
  [SerializeField]
  private Transform launchPoint;

  public void Launch(LaunchParam param) {
    if (param.bullet != null) {
      bulletPrefab = param.bullet;
    }

    if (bulletPrefab != null &&
        param.target != null) {
      var bulletObj = GameObject.Instantiate(bulletPrefab, launchPoint.transform.position, launchPoint.rotation);
      bulletObj.transform.LookAt(param.target.transform);
    }
  }

}
