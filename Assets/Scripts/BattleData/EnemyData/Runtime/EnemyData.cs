using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataHub;
using System;

[System.Serializable]
public struct EnemyData : IData<EnemyData> {
  private int version;

  public EnemyName enemyName;
  public int hp;
  [NonSerialized]
  public bool isDead;

  public bool HasDiff(EnemyData data) {
    return version != data.version;
  }

  public void UpdateVersion() {
    version++;
  }
}


/* TemplateId: 数据模板，同一个名字的敌人可以有多种模板（比如精英/非精英，或者别的东西）
 * Name：敌人的名字，用来对应敌人资源、信息
 */

public enum EnemyTemplateId {
  PapaWorm
}

public enum EnemyName {
  PapaWorm
}
