using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesLoader : MonoSingleton<ResourcesLoader> {
  [SerializeField]
  private TowerResourcesSet towerResourcesSet;
  [SerializeField]
  private EnemyResourcesSet enemyResourcesSet;
  public bool TryGetTowerResources(TowerName name, out TowerResources resources) {
    if (towerResourcesSet == null) {
      resources = null;
      return false;
    }
    return towerResourcesSet.TryGetValue(name, out resources);
  }

  public bool TryGetEnemyResources(EnemyName name, out EnemyResources resources) {
    if (enemyResourcesSet == null) {
      resources = null;
      return false;
    }
    return enemyResourcesSet.TryGetValue(name, out resources);
  }

}
