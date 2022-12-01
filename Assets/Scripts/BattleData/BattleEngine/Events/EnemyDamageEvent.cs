using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnmeyDamageType {
  Attack
}

public class EnemyDamageEvent : BattleEngine.Event {
  public EnmeyDamageType type;
  public int enemyPtr;
  public int towerPtr;
}
