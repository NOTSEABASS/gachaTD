using System;
[Serializable]
public class PoolObjectLifecycle {
  public string poolName;
  public Action OnSpawn;
  public Action OnDie;

  public PoolObjectLifecycle(string name) {
    this.poolName = name.ToLower();
  }
}
