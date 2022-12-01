using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  这个脚本用来实现敌人如何搜索目标
 *
 */
public class EnemySearchTarget : MonoSingleton<EnemySearchTarget> {

  public TowerBase SearchForCloset(Vector3 fromPosition) {
    TowerBase res = null;
    float maxDistance = -1;
    foreach (var tower in LifeCollector<TowerBase>.GetObjects()) {
      var ptr = tower.FindDataPtr();
      if (!TowerDataHub.Instance.TryGetData(ptr, out var data)) {
        continue;
      }

      if (data.isDead) {
        continue;
      }

      var d = Vector3.Distance(fromPosition, tower.transform.position);
      if (d > maxDistance) {
        res = tower;
        maxDistance = d;
      }
    }
    return res;
  }


}
