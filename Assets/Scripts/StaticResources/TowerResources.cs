using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
[CreateAssetMenu(menuName = MenuConsts.ScriptableObject + "Tower Resources")]
public class TowerResources : ScriptableObject {
  public string displayName;
  public GameObject prefab;
}
