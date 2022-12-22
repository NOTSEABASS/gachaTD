using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using System.CodeDom;
using DG.Tweening;
using UnityEditor.U2D;
using MyBox;

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
    data.isInMoveBatch = true;
    Debug.Assert(!isDying || IsDying == data.isDead);
    _cachedData = data;
    InitFunctionIfNot(data);
    inBattleSwitch.SetInBattle(data.isInBattle);
    DeathJudge(data);
    DoDeath();
  }

  private void InitFunctionIfNot(EnemyData data) {
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

  private void OnTriggerEnter(Collider other) {
    if (other.tag == TagConsts.Core) {
      isDying = true;
      EnemyCoreDamage.Instance.DealDamage(transform.position, cachedDataPtr);
      DoDeath();
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
    private EnemyBase self;
    private Transform transform;
    private EnemyBoidIdentity boidIdentity;

    private bool isMoving = true;
    public bool IsMoving => isMoving;

    private TowerBase target;
    public TowerBase Target => target;

    //params
    public float speed;
    public float stopDistance;
    public float interestRadius;

    public bool isValidBoid {
      set {
        if (boidIdentity == null ||
           (value && !boidIdentity.isValid)) {
          boidIdentity = new EnemyBoidIdentity(transform);
        }
        boidIdentity.isValid = value;
      }
    }

    public SearchAndMove_01(EnemyBase self) {
      this.self = self;
      transform = self.FindDataPtrObj().transform;
    }

    public void SetData(EnemyData data) {
      speed = data.moveSpeed;
      stopDistance = data.atkRadius;
      isValidBoid = data.isInBattle;
      interestRadius = data.interestRadius;
    }

    public void Update() {
      if (boidIdentity == null || !boidIdentity.isValid) {
        boidIdentity = new EnemyBoidIdentity(transform);
      }

      target = EnemySearchTarget.Instance.SearchForCloset(self.transform.position);
      if (target != null) {
        if (Vector3.Distance(transform.position, target.transform.position) > interestRadius) {
          target = null;
        }
      }

      Vector3 direction;
      if (target != null) {
        direction = target.transform.position - self.transform.position;
        isMoving = direction.magnitude > stopDistance;
      } else {
        direction = Vector3.zero - transform.position;
        isMoving = true;
      }


      if (!isMoving) {
        return;
      }

      transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
      var speedMove = speed * direction.normalized * Time.deltaTime;
      var boidMove = boidIdentity.boidForce * Time.deltaTime;
      var final = speedMove + boidMove;
      final.y = 0;
      transform.Translate(final, Space.World);
    }
  }



  #endregion 
}

