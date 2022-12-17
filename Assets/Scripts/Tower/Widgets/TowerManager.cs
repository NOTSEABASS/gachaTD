using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoSingleton<TowerManager> {
  public void ResetAllTower() {
    foreach (var tower in LifeCollector<TowerBase>.GetObjects()) {
      var ptr = tower.FindDataPtr();
      if (TowerDataHub.Instance.TryGetData(ptr, out var data)) {
        data.ResetLife();
        data.UpdateVersion();
        TowerDataHub.Instance.SetData(ptr, data);
      }
    }
  }

}
