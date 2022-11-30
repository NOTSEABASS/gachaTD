using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
[CreateAssetMenu(menuName =MenuConsts.ScriptableObject+"Enemy Resources")]
public class EnemyResources : ScriptableObject
{
  public string poolName;
  public string displayName;
}