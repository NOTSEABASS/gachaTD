using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDeployer_01 : MonoBehaviour {
  [SerializeField]
  private GameObject blockPrefab;
  [SerializeField]
  private Vector2Int count;

  [ContextMenu("Deploy")]
  public void Deploy() {
    var origin = new Vector3(-count.x / 2f + 0.5f, 0, -count.y / 2f + 0.5f);

    for (int z = count.y - 1; z > -1; z--) {
      for (int x = 0; x < count.x; x++) {
        var pos = new Vector3(x, 0, z) + origin;
        var obj = Instantiate(blockPrefab, transform);
        obj.transform.localPosition = pos;
      }
    }
  }
}
