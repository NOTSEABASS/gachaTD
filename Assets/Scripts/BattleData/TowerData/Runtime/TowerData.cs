using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataHub;

[System.Serializable]
public struct TowerData : IData<TowerData> {
  private int version;

  public int hp;
  public int maxHp;
  public int atk;
  public float atkFreq;
  public float atkRadius;
  public int energy;
  public int maxEnergy;
  public int energyRecover;

  public bool HasDiff(TowerData data) {
    return version != data.version;
  }

  public void UpdateVersion() {
    version++;
  }
}

public enum TowerDataTemplateId {
  Cannon,
  Buddy
}
