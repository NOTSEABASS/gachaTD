using DataHub;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHubTest : MonoBehaviour {
  public GameObject binderObj;
  private int ptr;

  void Start() {
    ptr = binderObj.GetInstanceID();
    var data = new TestData();
    data.val = 123;
    data.UpdateVersion();
    TestDataHub.Instance.RegisterData(ptr, data);
  }
}
