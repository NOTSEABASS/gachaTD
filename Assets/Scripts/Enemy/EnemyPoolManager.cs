using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine.Pool;
using UnityEngine;

public class EnemyPoolManager : MonoSingleton<EnemyPoolManager> {
  [SerializeField] private int poolMaxmumSize;
  private Dictionary<string, ObjectPool<GameObject>> enemyPoolDict;
  [SerializeField] private EnemyScriptableObjectList enemyList;

  protected override void Awake() {
    base.Awake();
    enemyPoolDict = new Dictionary<string, ObjectPool<GameObject>>();
  }

  // Start is called before the first frame update
  void Start()
  {
    foreach (var enemy in enemyList.list) {
      Debug.Log("Construct " + enemy.name);
      Func<GameObject> create = () => {
        Debug.Log("Create");
        var go = Instantiate(enemyList[enemy.name.ToLower()].prefab, Vector3.zero, Quaternion.identity);
        go.GetComponent<EnemyBase>().polc.poolName = enemy.name;
        return go;
      };
      var pool = new ObjectPool<GameObject>(create, PoolGet, PoolRelease, PoolDestroy, true, 10, poolMaxmumSize);
      enemyPoolDict.Add(enemy.name.ToLower(), pool);
    }
  }

  public void Get(string name) {
    name = name.ToLower();
    if (!enemyList.ContainsKey(name)) {
      Debug.LogWarning("ObjectPool " + name + " Not Exist");
      return;
    }

    enemyPoolDict[name].Get();
  }

  public void Release(string poolName,GameObject gameObject) {
    poolName = poolName.ToLower();
    if (!enemyList.ContainsKey(poolName)) {
      Debug.LogWarning("ObjectPool " + poolName + " Not Exist");
      return;
    }
    enemyPoolDict[poolName].Release(gameObject);
  }

  private void PoolGet(GameObject gameObject) {
    gameObject.SetActive(true);
  }

  private void PoolRelease(GameObject gameObject) {
    gameObject.SetActive(false);
  }

  private void PoolDestroy(GameObject gameObject) {
    Destroy(gameObject);
  }
  
}
