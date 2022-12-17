using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolGetter<T> where T : PoolObject {
  public string poolName;

  public bool TryGet(out T obj) {
    if (PoolManager.Instance == null) {
      Debug.LogError("[PoolGetter] PoolManager Not Exists");
      obj = null;
      return false;
    }

    if (PoolManager.Instance.TryGetObjectByPoolName<T>(poolName, out obj)) {
      return true;
    }
    Debug.LogError("[PoolGetter] can't get object from pool manager");
    obj =null;
    return false;
  }
}
