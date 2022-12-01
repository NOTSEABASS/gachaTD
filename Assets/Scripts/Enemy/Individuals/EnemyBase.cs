using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering.Universal;
using UnityEngine.Events;
using TreeEditor;

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
  private bool hasDataPtr;
  protected int cachedDataPtr {
    get {
      if (!hasDataPtr) {
        hasDataPtr = true;
        dataPtr = this.FindDataPtr();
      }
      return dataPtr;
    }
  }

  private EnemyPoolObject poolObject;
  private bool isDying;
  private bool didDeath;
  public bool IsDying => isDying;

  protected virtual void Awake() {
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



  public void OnRelease() {
    isDying = false;
    didDeath = false;
  }

  #region Common Function
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


  #endregion

  #region Common Class
  protected class SearchAndMove_01 {
    private const float stopDistanceBuff = 0.2f; //从停止到重新开始移动的缓冲距离
    private EnemyBase self;
    private Transform transform;

    private bool isMoving = true;
    public bool IsMoving => isMoving;

    private TowerBase target;
    public TowerBase Target => target;

    public float speed;
    public float stopDistance;
    public SearchAndMove_01(EnemyBase self) {
      this.self = self;
      transform = self.FindDataPtrObj().transform;
    }

    public void Update() {
      target = EnemySearchTarget.Instance.SearchForCloset(self.transform.position);
      if (target == null) {
        return;
      }

      var toward = target.transform.position - self.transform.position;

      isMoving = toward.magnitude > stopDistance;
      if (!isMoving) {
        return;
      }

      toward.y = 0;
      transform.rotation = Quaternion.LookRotation(toward, Vector3.up);

      transform.Translate(speed * toward.normalized * Time.deltaTime, Space.World);

    }


  }
  #endregion 
}

