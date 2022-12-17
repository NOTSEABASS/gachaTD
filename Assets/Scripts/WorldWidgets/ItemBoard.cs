using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBoard : MonoSingleton<ItemBoard> {
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
  }

}
