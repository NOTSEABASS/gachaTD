using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class EnemyDamageSystem : BattleEngine.System {
  public override void Handle(BattleEngine.Event eve) {
    if (!(eve is EnemyDamageEvent ede)) {
      return;
    }
    if (!towerDataHub.TryGetData(ede.towerPtr, out var towerData)) {
      Debug.LogError("cant get towerData");
      return;
    }
    if (!enemyDataHub.TryGetData(ede.enemyPtr, out var enemyData)) {
      Debug.LogError("cant get enemyData");
      return;
    }

    if (ede.type == EnmeyDamageType.Attack) {
      towerData.hp -= enemyData.atk;
      if (towerData.hp <= 0) {
        towerData.isDead = true;
      }
      towerData.UpdateVersion();
      towerDataHub.SetData(ede.towerPtr, towerData);
    }
  }
}
