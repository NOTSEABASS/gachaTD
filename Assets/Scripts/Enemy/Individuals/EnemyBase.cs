using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using System.CodeDom;

public class EnemyBase : MonoBehaviour, IPoolCallback {
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

  private GameObject dataPtrObj;
  protected GameObject cachedDataPtrObj {
    get {
      if (dataPtrObj == null) {
        dataPtrObj = this.FindDataPtrObj();
      }
      return dataPtrObj;
    }
  }

  private EnemyData _cachedData;
  protected EnemyData cachedData => _cachedData;

  private EnemyPoolObject poolObject;
  private bool isDying;
  private bool didDeath;
  public bool IsDying => isDying;

  private InBattleSwitch inBattleSwitch;

  protected virtual void Awake() {
    dataPtr = this.FindDataPtr();
    inBattleSwitch = new InBattleSwitch(cachedDataPtrObj);
    if (!this.FindDataPtrObj().TryGetComponent(out poolObject)) {
      Debug.LogError("can't find PoolObject on enemy DataPtrObj");
    }
  }

  public virtual void OnDataChange(EnemyData data) {
    Debug.Assert(!isDying || IsDying == data.isDead);

    _cachedData = data;
    InitIfNot(data);
    inBattleSwitch.SetInBattle(data.isInBattle);
    DeathJudge(data);
    DoDeath();
  }

  private void InitIfNot(EnemyData data) {
    if (!data.hasInited) {
      data.hasInited = true;
      EnemyMoveBatchManager.Instance.Register(data.moveBatchIndex, cachedDataPtr);
      data.UpdateVersion();
      EnemyDataHub.Instance.SetData(cachedDataPtr, data);
      LookAtCenter();

    }
  }

  public void OnRelease() {
    isDying = false;
    didDeath = false;
    hasDataPtr = false;
    dataPtrObj = null;
  }

  #region Common Function
  protected void LookAtCenter() {
    var transform = cachedDataPtrObj.transform;
    transform.LookAt(Vector3.zero);
    var forward = transform.forward;
    forward.y = 0;
    transform.forward = forward;
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


  #endregion

  #region Common Class
  [Serializable]
  public class UtilFunction {
    public float deathDelay;
    public UnityEvent onDeath;
  }

  protected class InBattleSwitch {
    //将一个怪物生成出来而让其不受战斗影响，则将其所有能被触发的碰撞盒关掉
    private List<Collider> logicColliders = new List<Collider>();

    public InBattleSwitch(GameObject gameObject) {
      var collidersInChildren = gameObject.GetComponentsInChildren<Collider>();
      var layer = LayerConsts.EnemyLayer;
      foreach (var collider in collidersInChildren) {
        if (collider.gameObject.layer == layer) {
          collider.enabled = false;
          logicColliders.Add(collider);
        }
      }
    }

    public void SetInBattle(bool isInBattle) {
      foreach (var collider in logicColliders) {
        collider.enabled = isInBattle;
      }
    }

  }


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

