using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBoard_01 : MonoSingleton<ItemBoard_01> {
  [SerializeField]
  private Transform sortingAnchor;
  private List<Transform> draggingMounts = new List<Transform>();
  public int emptySlotCount {
    get {
      int cnt = 0;
      foreach (var mount in draggingMounts) {
        if (mount.childCount == 0) {
          cnt++;
        }
      }
      return cnt;
    }
  }

  public Transform GetEmptySlot() {
    foreach (var mount in draggingMounts) {
      if (mount.childCount == 0) {
        return mount;
      }
    }
    return null;
  }

  protected override void Awake() {
    base.Awake();
    var children = GetComponentsInChildren<Transform>();
    foreach (var child in children) {
      if (child.gameObject.layer == LayerConsts.DraggingMountLayer) {
        draggingMounts.Add(child);
      }
    }
    draggingMounts.Sort((a, b) => {
      var anchor = sortingAnchor.position;
      var da = Vector3.Distance(anchor, a.position);
      var db = Vector3.Distance(anchor, b.position);
      return Math.Sign(da - db);
    });
  }

}
