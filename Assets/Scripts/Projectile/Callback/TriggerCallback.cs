using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//碰撞触发规则，只碰撞一次、持续碰撞、有限次碰撞等
public enum TriggerRule {
  SingleUse
}

public enum TriggerType {
  Enter,
  Stay,
  Exit
}

[Serializable]
public struct TriggerContext {
  public TriggerType type;
  public Collider otherCollider;
}

public class TriggerCallback : MonoBehaviour {
  [SerializeField]
  private TriggerRule rule;
  [SerializeField]
  private UnityEvent<TriggerContext> localCallback;

  private Action<TriggerContext> dynCallback; // 代码中指定的回调

  public void SetDynCallback(Action<TriggerContext> callback) {
    dynCallback = callback;
  }

  private void InvokeCallbacks(TriggerContext context) {
    localCallback?.Invoke(context);
    dynCallback?.Invoke(context);
  }

  private void OnTriggerEnter(Collider other) {
    if (rule == TriggerRule.SingleUse) {
      var context = new TriggerContext() {
        type = TriggerType.Enter,
        otherCollider = other
      };
      InvokeCallbacks(context);
    }
  }

  private void OnTriggerExit(Collider other) {
    if (rule == TriggerRule.SingleUse) {
      var context = new TriggerContext() {
        type = TriggerType.Exit,
        otherCollider = other
      };
      InvokeCallbacks(context);
    }
  }
}
