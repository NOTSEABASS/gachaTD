using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MenuConsts.ScriptableObject + "Tower Template")]
public class TowerTemplate : ScriptableObject
{
  [SerializeField] 
  private TowerData towerData;

  public virtual TowerData GetData() {
    return towerData;
  }

}
