using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMountDeployer : MonoBehaviour {
  [SerializeField]
  private GameObject mountPrefab;
  [SerializeField]
  private Vector2Int range;

  private void Awake() {
    var right = transform.right;
    var forward = transform.forward;

    for (int x = 0; x < range.x; x++) {
      for (int y = 0; y < range.y; y++) {
        var pos = right * x + forward * y;
        var newObj = Instantiate(mountPrefab, transform);
        newObj.transform.position += x * right + y * forward;
      }
    }
    mountPrefab.SetActive(false);
  }
}
