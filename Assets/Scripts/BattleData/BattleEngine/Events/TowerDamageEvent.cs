using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TowerDamageType {
  Attack
}

public class TowerDamageEvent : BattleEngine.Event {
  public TowerDamageType type;
  public int enemyPtr;
  public int towerPtr;
}
