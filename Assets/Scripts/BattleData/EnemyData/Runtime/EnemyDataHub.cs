using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataHub;

public class EnemyDataHub : DataHub<EnemyData> {
  protected override void OnBeforeRegisterData(int ptr, ref EnemyData data) {
    data.ptr = ptr;
  }

  protected override void OnBeforeSetData(int ptr, ref EnemyData data) {
    data.ptr = ptr;
  }
}
