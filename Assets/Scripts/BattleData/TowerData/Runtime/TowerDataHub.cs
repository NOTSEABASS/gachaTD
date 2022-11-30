using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataHub;

public class TowerDataHub : DataHub<TowerData> {
  protected override void OnBeforeRegisterData(int ptr, ref TowerData data) {
    data.ptr = ptr;
  }

  protected override void OnBeforeSetData(int ptr, ref TowerData data) {
    data.ptr = ptr;
  }
}
