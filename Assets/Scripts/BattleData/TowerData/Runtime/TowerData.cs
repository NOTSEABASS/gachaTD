using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataHub;

[System.Serializable]
public struct TowerData : IData<TowerData> {
  private int version;

  public int hp;
  public int atk;
  public float atkInterval;
  public float atkRadius;

  public bool HasDiff(TowerData data) {
    return version != data.version;
  }

  public void UpdateVersion() {
    version++;
  }
}

public enum TowerDataTemplateId {
  Cannon
}
