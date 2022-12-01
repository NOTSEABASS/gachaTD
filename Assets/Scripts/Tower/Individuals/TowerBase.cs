using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Drawing.Inspector.PropertyDrawers;
using UnityEngine;

public abstract class TowerBase : MonoBehaviour {
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

  protected virtual void Awake() {
    LifeCollector<TowerBase>.AddObject(this);
  }

  public abstract void OnDataChange(TowerData data);
}