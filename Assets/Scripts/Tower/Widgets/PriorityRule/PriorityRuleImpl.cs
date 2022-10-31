using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PriorityRuleImpl {


  public static DetectResult Solve(Collider[] colliders, DetectParam param) {
    switch (param.priorityRule) {
      case PriorityRule.Closet:
        return Closet(colliders, param);
      default:
        throw new System.Exception();
    }
  }

  private static DetectResult Closet(Collider[] colliders, DetectParam param) {
    Collider closet = null;
    foreach (var collider in colliders.SafeUObjects()) {
      if (closet == null ||
          Vector3.Distance(collider.transform.position, param.position) <
          Vector3.Distance(closet.transform.position, param.position)) {
        closet = collider;
      }
    }

    return new DetectResult {
      singleResult = closet.transform.FindDataPtrObj()
    };
  }

}
