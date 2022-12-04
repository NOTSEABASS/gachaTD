using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataHub;
using System;

[System.Serializable]
public struct EnemyData : IData<EnemyData> {
  private int version;

  public EnemyName name;
  public int hp;
  public int maxHp;

  public int atk;
  public float atkRadius;
  public float atkFreq;

  public float moveSpeed;

  [NonSerialized]
  public bool isDead;
  [NonSerialized]
  public int ptr;
  [NonSerialized]
  public bool hasInited; //每次从池中取出后是否初始化
  [NonSerialized]
  public int moveBatchIndex;  //属于第几小波怪

  [NonSerialized]
  public bool isInBattle;  //是否已经在逻辑上进入战斗


  public bool HasDiff(EnemyData data) {
    return version != data.version;
  }

  public void UpdateVersion() {
    version++;
  }

}
/*warning: 
 * 是否会存在一种极端情况，被设置isDead的对象在死亡后立刻被取出池，
 * 导致上一条命相关的组件还未读到这个isDead，这个ptr指向的数据就被下一条命覆盖了
 */

/* TemplateId: 数据模板，同一个名字的敌人可以有多种模板（比如精英/非精英，或者别的东西）
 * Name：敌人的名字，用来对应敌人资源、信息
 */

public enum EnemyTemplateId {
  PapaWorm,
  Chicken,
  Wizard,
}

public enum EnemyName {
  PapaWorm,
  Chicken,
  Wizard,
}
