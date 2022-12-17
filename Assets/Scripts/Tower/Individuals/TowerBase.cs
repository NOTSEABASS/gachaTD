using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;


public interface ITowerLifeHandler {
  public void OnTowerLifeReset();
  public void OnTowerDie();
}

public abstract class TowerBase : MonoBehaviour {
  private int dataPtr;
  private bool hasDataPtr;
  private ITowerLifeHandler[] lifeHandlers;
  protected int cachedDataPtr {
    get {
      if (!hasDataPtr) {
        hasDataPtr = true;
        dataPtr = this.FindDataPtr();
      }
      return dataPtr;
    }
  }

  protected TowerData cachedData;


  protected virtual void Awake() {
    LifeCollector<TowerBase>.AddObject(this);
    var rootObj = this.FindDataPtrObj();
    lifeHandlers = rootObj.GetComponentsInChildren<ITowerLifeHandler>();
  }

  public virtual void OnDataChange(TowerData data) {
    if (cachedData.isDead != data.isDead) {
      if (data.isDead) {
        foreach (var handler in lifeHandlers) {
          handler.OnTowerDie();
        }
      } else {
        foreach (var handler in lifeHandlers) {
          handler.OnTowerLifeReset();
        }
      }
    }
    cachedData = data;
  }
}