using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleDetector : Detector {
  private const float CAPSULE_HEIGHT = 20;
  public float radius;

  protected override DetectResult DoDetect(DetectParam param) {
    var pos0 = param.position + Vector3.up * CAPSULE_HEIGHT / 2;
    var pos1 = param.position + Vector3.down * CAPSULE_HEIGHT / 2;

    var colliders = Physics.OverlapCapsule(pos0, pos1, radius, param.layerMask, QueryTriggerInteraction.Ignore);
    if(colliders.Length == 0) {
      return new DetectResult();
    }

    return PriorityRuleImpl.Solve(colliders,param);
  }
}
