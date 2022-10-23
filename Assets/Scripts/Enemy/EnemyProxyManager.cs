using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//存储用的数据结构，自动索引proxy类和gameobject
public class EnemyObject {
  public GameObject gameObject;
  public EnemyProxyBase enemyProxy;

  public EnemyObject(GameObject go, EnemyProxyBase enemyProxy) {
    this.gameObject = go;
    this.enemyProxy = enemyProxy;
  }
}

//用来代理生成的Enemy的行为
public abstract class EnemyProxyBase {
  public Transform transform;
  public PoolObject polc;

  public EnemyProxyBase(Transform transform) {
    this.transform = transform;
  }
  public abstract void OnUpdate();
}

//用来管理所有现有存活的EnemyProxy
public class EnemyProxyManager : MonoSingleton<EnemyProxyManager> {
  private Dictionary<int,EnemyObject> enemyObjects;
  // Start is called before the first frame update
  void Start() {
    enemyObjects = new Dictionary<int, EnemyObject>();
  }

  // Update is called once per frame
  void Update()
  {
    foreach (var eo in enemyObjects) {
      eo.Value.enemyProxy.OnUpdate();
    }
  }

  public void AddEnemy(EnemyObject eo) {
    enemyObjects.Add(eo.gameObject.GetInstanceID(),eo);
  }

  public void RemoveEnemy(EnemyObject eo) {
    enemyObjects.Remove(eo.gameObject.GetInstanceID());
  }
}
