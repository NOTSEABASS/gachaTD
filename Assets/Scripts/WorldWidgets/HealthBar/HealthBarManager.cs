using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarManager : MonoBehaviour {
  private const string healthBarPoolName = "healthbar";
  private List<EnemyData> interateBuffer = new List<EnemyData>();
  private void Update() {
    UpdateEnemyHealthBar();
  }

  private void UpdateEnemyHealthBar() {
    int idx = 0;
    foreach (EnemyData data in EnemyDataHub.Instance.AllDatas) {
      if (idx < interateBuffer.Count) {
        interateBuffer[idx++] = data;
      } else {
        interateBuffer.Add(data);
      }
    }

    foreach (var data in interateBuffer) {
      if (data.isDead || data.hasHealthBar) {
        continue;
      }

      if (PoolManager.Instance.TryGetObjectByPoolName<HealthBar>(healthBarPoolName, out var healthBar)) {
        healthBar.transform.parent = data.gameObject.transform;
        healthBar.transform.localPosition = Vector3.zero;
        healthBar.dataPtr = data.ptr;
        healthBar.functionType = HealthBar.FunctionType.Enemy;
      }

      var tmp = data;
      tmp.hasHealthBar = true;
      EnemyDataHub.Instance.SetData(tmp.ptr, tmp);
    }
  }
}
