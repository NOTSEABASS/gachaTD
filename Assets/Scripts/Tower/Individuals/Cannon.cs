using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : TowerBase {
  [SerializeField]
  private RotationAiming rotationAiming;
  [SerializeField]
  private StraightLauncher launcher = new StraightLauncher();

  private FixedClock attackClock = new FixedClock();
  private CircleDetector attackDetector = new CircleDetector();

  public override void OnDataChange(TowerData data) {
    base.OnDataChange(data);
    attackClock.freq = data.atkFreq;
    attackDetector.radius = data.atkRadius;
  }

  protected void FixedUpdate() {
    attackClock.Update(Time.fixedDeltaTime);

    var detectParam = new DetectParam {
      position = transform.position,
      layerMask = LayerConsts.EnemyLayer,
      priorityRule = PriorityRule.Closet
    };

    var detetecResult = attackDetector.Detect(detectParam);

    if (detetecResult.singleResult != null) {
      rotationAiming.Aim(detetecResult.singleResult);
    }

    if (detetecResult.singleResult != null && attackClock.isReady) {
      attackClock.OnTrigger();
      var damageEvent = new TowerDamageEvent() {
        type = TowerDamageType.Attack,
        towerPtr = cachedDataPtr
      };

      var launchParam = new LaunchParam {
        target = detetecResult.singleResult,
        projectilePlugin = new ProjectilePlugin.TowerTouchDamage(damageEvent)
      };

      launcher.Launch(launchParam);
    }
  }

}

