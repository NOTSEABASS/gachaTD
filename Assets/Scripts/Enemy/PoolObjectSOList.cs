using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PoolObjectSource {
  public GameObject prefab;
}

[CreateAssetMenu(menuName = "ScriptableObject/PoolObjectSourceSet",fileName = "PoolObjectSource")]
public class PoolObjectSOList : ScriptableObjectSet<PoolObject> {
  
}
