using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering.Universal;
using UnityEngine.Events;

public class EnemyBase : MonoBehaviour, IPoolCallback {
  #region Inner Class
  [Serializable]
  public class UtilFunction {
    public float deathDelay;
    public UnityEvent onDeath;
  }
  #endregion

  [SerializeField]
  private UtilFunction utilFunction;

  private int dataPtr;
  private EnemyPoolObject poolObject;
  private bool isDying;
  private bool didDeath;
  public bool IsDying => isDying;

  private void Awake() {
    dataPtr = this.FindDataPtr();
    if (!this.FindDataPtrObj().TryGetComponent(out poolObject)) {
      Debug.LogError("can't find PoolObject on enemy DataPtrObj");
    }
  }

  public virtual void OnDataChange(EnemyData data) {
    Debug.Assert(!isDying || IsDying == data.isDead);
    DeathJudge(data);
    DoDeath();
  }

  protected virtual void DeathJudge(EnemyData data) {
    if (data.isDead) {
      isDying = true;
    }
  }

  protected virtual void DoDeath() {
    if (isDying && !didDeath) {
      didDeath = true;
      var deathEvent = new EnemyDeathEvent();
      deathEvent.enemyPtr = dataPtr;
      BattleEngine.Instance.PushEvent(deathEvent);
      poolObject.DelayReleaseToPool(utilFunction.deathDelay);
      utilFunction.onDeath.Invoke();
    }
  }

  public void OnRelease() {
    isDying = false;
    didDeath = false;
  }
}
