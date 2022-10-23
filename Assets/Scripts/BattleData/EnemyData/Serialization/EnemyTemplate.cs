using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MenuConsts.ScriptableObject + "Enemy Template")]
public class EnemyTemplate : ScriptableObject {
  [SerializeField]
  private EnemyData enemyData;

  public virtual EnemyData GetData() {
    return enemyData;
  }
}