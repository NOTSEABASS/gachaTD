using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DetectParam {
  public Vector3 position;
  public LayerMask layerMask;
  public PriorityRule priorityRule;
}

public struct DetectResult {
  public GameObject singleResult;
  public GameObject[] results;
}

public enum PriorityRule {
  Closet
}