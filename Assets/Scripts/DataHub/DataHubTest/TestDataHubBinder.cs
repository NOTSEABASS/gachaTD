using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDataHubBinder : TestDataHub.Binder {
  protected override void OnDataUpdate(TestData td) {
    print($"data update: {td.val}");
  }
}
