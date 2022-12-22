using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Plugin = ProjectilePoolObject.Plugin;
public static class ProjectilePlugin {
  public class TowerTouchDamage : Plugin {
    private TowerDamageEvent cachedDamageEvent;

    public TowerTouchDamage(TowerDamageEvent cachedDamageEvent) {
      this.cachedDamageEvent = cachedDamageEvent;
    }

    public override void OnSetPlugin() {
      triggerCallback.SetDynCallback(OnTrigger);
    }

    private void OnTrigger(TriggerContext context) {
      cachedDamageEvent.enemyPtr = context.otherCollider.FindDataPtr();

      BattleEngine.Instance.PushEvent(cachedDamageEvent);

      poolObject.ReleaseToPool();
    }
  }

  public class TowerLockDamage : Plugin {
    private TowerDamageEvent cachedDamageEvent;
    private Transform target;
    private LockFly lockFly;
    public TowerLockDamage(TowerDamageEvent cachedDamageEvent, Transform target) {
      this.cachedDamageEvent = cachedDamageEvent;
      this.target = target;
    }

    public override void OnSetPlugin() {
      lockFly = poolObject.GetComponent<LockFly>();
      Debug.Assert(lockFly != null);
      lockFly.target = target;
    }

    public override void OnUpdate() {
      if (lockFly.isArrived) {
        if (lockFly.HasValidTarget()) {
          BattleEngine.Instance.PushEvent(cachedDamageEvent);
        }
        poolObject.ReleaseToPool();
      }
    }
  }
}
