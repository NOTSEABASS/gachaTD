using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public struct HealthBarStyle {
  public const int Enemy = 0;
  public const int Tower = 1;

  public int type;
  public float offsetHeight;
}

public class HealthBarController : MonoBehaviour {
  [SerializeField]
  private Transform root;
  [SerializeField]
  private Transform scale;
  [SerializeField]
  private GameObject[] fronts;

  public void SetNormalizedScale(float n) {
    n = Mathf.Clamp01(n);
    scale.SetLocalScaleX(n);
  }

  public void SetStyle(HealthBarStyle style) {
    Debug.Assert(style.type > -1 && style.type < fronts.Length);
    ActiveFront(style.type);
    var localPos = root.localPosition;
    localPos.y = style.offsetHeight;
    root.localPosition = localPos;
  }

  private void ActiveFront(int i) {
    for (int j = 0; j < fronts.Length; j++) {
      fronts[j].SetActive(j == i);
    }
  }
}
