using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthBar : PoolObject {
  public enum FunctionType {
    Tower, Enemy
  }

  [SerializeField]
  private HealthBarController healthBarController;
  private FunctionType _functionType;
  public FunctionType functionType {
    set {
      _functionType = value;
      UpdateStyle();
    }
  }
  public int dataPtr { set; private get; }


  public void Update() {
    switch (_functionType) {
      case FunctionType.Tower:
        TowerFunction();
        break;
      case FunctionType.Enemy:
        EnemyFunction();
        break;
      default:
        break;
    }
  }

  private void UpdateStyle() {
    HealthBarStyle style = default;
    switch (_functionType) {
      case FunctionType.Tower:
        style = new HealthBarStyle {
          type = HealthBarStyle.Tower,
          offsetHeight = 0.5f
        };
        break;
      case FunctionType.Enemy:
        style = new HealthBarStyle {
          type = HealthBarStyle.Enemy,
          offsetHeight = 0.5f
        };
        break;
    }
    healthBarController.SetStyle(style);
  }

  private void TowerFunction() {
    if (TowerDataHub.Instance.TryGetData(dataPtr, out var data)) {
      if (data.isDead) {
        ReleaseToPool();
        return;
      }
      var n = (float)data.hp / data.maxHp;
      healthBarController.SetNormalizedScale(n);
    }
  }

  private void EnemyFunction() {
    if (EnemyDataHub.Instance.TryGetData(dataPtr, out var data)) {
      if(data.isDead){ 
        ReleaseToPool();
        return;
      }
      var n = (float)data.hp / data.maxHp;
      healthBarController.SetNormalizedScale(n);
    }
  }

}
