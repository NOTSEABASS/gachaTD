using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemySource {
  public GameObject prefab;
}

[CreateAssetMenu(menuName = "ScriptableObject/EnemySourceSet",fileName = "EnemySource")]
public class EnemyScriptableObjectList : ScriptableObjectSet<EnemySource> {
  
}
