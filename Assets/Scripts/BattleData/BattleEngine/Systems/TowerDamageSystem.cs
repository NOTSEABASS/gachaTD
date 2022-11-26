using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class TowerDamageSystem : BattleEngine.System {
  public override void Handle(BattleEngine.Event eve) {
    if (!(eve is TowerDamageEvent tde)) {
      return;
    }
    if (!towerDataHub.TryGetData(tde.towerPtr, out var towerData)) {
      Debug.LogError("cant get towerData");
      return;
    }
    if (!enemyDataHub.TryGetData(tde.enemyPtr, out var enemyData)) {
      Debug.LogError("cant get enemyData");
      return;
    }

    if (tde.type == TowerDamageType.Attack) {
      enemyData.hp -= towerData.atk;
      if (enemyData.hp <= 0) {
        enemyData.isDead = true;
      }
      enemyData.UpdateVersion();

      enemyDataHub.SetData(tde.enemyPtr, enemyData);
    }
  }
}
