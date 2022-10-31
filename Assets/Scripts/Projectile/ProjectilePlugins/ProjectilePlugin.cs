using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProjectilePlugin {
  public class TowerDamage : ProjectilePoolObject.Plugin {
    private TowerDamageEvent cachedDamageEvent;

    public TowerDamage(TowerDamageEvent cachedDamageEvent) {
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
}
