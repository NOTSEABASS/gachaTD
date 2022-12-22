using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailReset : MonoBehaviour, IPoolCallback {
  [SerializeField, AutoProperty]
  private TrailRenderer trail;

  public void OnRelease() {
    trail.Clear();
  }
}
