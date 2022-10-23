using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataHub;

public struct TestData : IData<TestData> {
  private int version;

  public int val;

  public bool HasDiff(TestData data) {
    return data.version != version;
  }

  public void UpdateVersion() {
    version++;
  }
}

public class TestDataHub : DataHub<TestData> {
 
}
