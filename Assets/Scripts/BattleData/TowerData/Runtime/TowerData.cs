using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataHub;
using System;

[System.Serializable]
public struct TowerData : IData<TowerData> {
  private int version;

  public TowerName name;
  public int hp;
  public int maxHp;
  public int atk;
  public float atkFreq;
  public float atkRadius;
  public int energy;
  public int maxEnergy;
  public int energyRecover;

  [NonSerialized]
  public int ptr;
  [NonSerialized]
  public bool isDead;
  [NonSerialized]
  public bool isInBattle;

  public bool HasDiff(TowerData data) {
    return version != data.version;
  }

  public void UpdateVersion() {
    version++;
  }
}

/* TemplateId: 数据模板，同一个名字的敌人可以有多种模板（比如精英/非精英，或者别的东西）
 * Name：小人的名字，用来对应小人资源、信息
 */


public enum TowerName {
  Buddy
}

public enum TowerDataTemplateId {
  Cannon,
  Buddy
}


