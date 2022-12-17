using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolObjectCreator : MonoBehaviour {
  [SerializeField]
  private string poolName;
  [SerializeField]
  private PoolObjectSOList poolObjectSOList;
  [SerializeField]
  private Vector3 position;
  private void OnGUI() {
    if (WidgetGUILayout.Button($"Create [{poolName}]", gameObject.GetInstanceID().ToString())) {
      PoolManager.Instance.TryGetObjectByPoolName<EnemyPoolObject>(poolName, out var obj);
      obj.transform.position = position;
    }
  }
}
