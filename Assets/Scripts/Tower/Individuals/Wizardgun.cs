using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class Wizardgun : TowerBase {
  [SerializeField]
  private RotationAiming rotationAiming;
  [SerializeField]
  private StraightLauncher launcher_0 = new StraightLauncher();
  [SerializeField]
  private StraightLauncher launcher_1 = new StraightLauncher();

  [SerializeField]
  private Transform stick;
  [SerializeField]
  private float recoilStrength;
  private Vector3 stickOrigin;

  private StraightLauncher[] launchers;
  private int launcherIdx;

  private FixedClock attackClock = new FixedClock();
  private CircleDetector attackDetector = new CircleDetector();

  protected override void Awake() {
    base.Awake();
    launchers = new StraightLauncher[] { launcher_0, launcher_1 };
    stickOrigin = stick.transform.localPosition;
  }

  public override void OnDataChange(TowerData data) {
    base.OnDataChange(data);
    attackClock.freq = data.atkFreq;
    attackDetector.radius = data.atkRadius;
  }

  protected void FixedUpdate() {
    if (!cachedData.isInBattle) {
      return;
    }

    attackClock.Update(Time.fixedDeltaTime);

    var detectParam = new DetectParam {
      position = transform.position,
      layerMask = LayerConsts.EnemyMask,
      priorityRule = PriorityRule.Closet
    };

    var detetecResult = attackDetector.Detect(detectParam);

    if (detetecResult.singleResult != null) {
      rotationAiming.Aim(detetecResult.singleResult);
    }

    var lerp = CurveUtils.ParabolicLerp(attackClock.normalizedTime);

    stick.transform.localPosition = stickOrigin + lerp * recoilStrength * Vector3.up;

    if (detetecResult.singleResult != null && attackClock.isReady) {
      attackClock.OnTrigger();
      var damageEvent = new TowerDamageEvent() {
        type = TowerDamageType.Attack,
        towerPtr = cachedDataPtr,
        enemyPtr = detetecResult.singleResult.FindDataPtr()
      };

      var launchParam_0 = new LaunchParam {
        target = detetecResult.singleResult,
        projectilePlugin = new ProjectilePlugin.TowerLockDamage(damageEvent, detetecResult.singleResult.transform)
      };

      launcherIdx++;
      launcherIdx %= 2;
      var launcher = launchers[launcherIdx];
      launcher.Launch(launchParam_0);
    }
  }

  private void SetStickRecoil(float n) {

  }

}
