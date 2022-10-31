using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(TriggerCallback))]
public class ProjectilePoolObject : PoolObject {
  #region Inner Classes
  public abstract class Plugin {
    public TriggerCallback triggerCallback;
    public ProjectilePoolObject poolObject;

    public abstract void OnSetPlugin();
  }
  #endregion



  protected override void Awake() {
    base.Awake();
  }

  public void SetPlugin(Plugin plugin) {
    plugin.poolObject = this;
    plugin.triggerCallback = GetComponent<TriggerCallback>();
    plugin.OnSetPlugin();
  }

}
