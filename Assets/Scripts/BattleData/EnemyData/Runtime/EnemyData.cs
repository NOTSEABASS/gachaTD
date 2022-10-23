using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataHub;

[System.Serializable]
public struct EnemyData : IData<EnemyData> {
  private int version;

  public int hp;

  public bool HasDiff(EnemyData data) {
    return version != data.version;
  }

  public void UpdateVersion() {
    version++;
  }
}

public enum EnemyTemplateId {
  PapaWorm
}
