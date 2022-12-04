using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PapaWorm : EnemyBase {
  [SerializeField]
  private DDAnimator DDAnimator;
  [SerializeField]
  private float atkAnimationOffset;
  [SerializeField]
  private float runAnimationLoopTime;

  private FixedClock attackClock = new FixedClock();

  private SearchAndMove_01 searchAndMove;

  protected override void Awake() {
    base.Awake();
    searchAndMove = new SearchAndMove_01(this);
  }

  public override void OnDataChange(EnemyData data) {
    base.OnDataChange(data);
    searchAndMove.speed = data.moveSpeed;
    searchAndMove.stopDistance = data.atkRadius;
    attackClock.freq = data.atkFreq;
  }

  private void Update() {
    if (!cachedData.isInBattle) {
      return;
    }
    searchAndMove.Update();
  }

  private void FixedUpdate() {
    if (!cachedData.isInBattle) {
      return;
    }

    attackClock.Update(Time.fixedDeltaTime);

    bool hasTarget = searchAndMove.Target != null;

    if (!hasTarget || searchAndMove.IsMoving) {
      DDAnimator.SetUpdateMode(DDAnimator.UpdateMode.Loop, runAnimationLoopTime);
      DDAnimator.SetState("Run", 0);
    } else {
      DDAnimator.SetUpdateMode(DDAnimator.UpdateMode.Manual);
      DDAnimator.SetState("Attack", atkAnimationOffset);
      DDAnimator.Play(attackClock.normalizedTime);
      if (attackClock.isReady) {
        attackClock.OnTrigger();

        var damageEvent = new EnemyDamageEvent() {
          enemyPtr = cachedDataPtr,
          towerPtr = searchAndMove.Target.FindDataPtr(),
          type = EnmeyDamageType.Attack
        };

        BattleEngine.Instance.PushEvent(damageEvent);
      }
    }
  }
}
