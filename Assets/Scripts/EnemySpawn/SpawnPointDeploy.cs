using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointDeploy : MonoBehaviour {
  [SerializeField]
  private int range;
  [SerializeField]
  private float step;

  [ContextMenu("Deploy")]
  public void Deploy() {
    for (int i = -range; i <= range; i++) {
      var point = new GameObject("Deploy");
      point.transform.position = transform.position;
      point.transform.position += i * transform.right * step;
      point.transform.parent = transform;
    }
  }
}
