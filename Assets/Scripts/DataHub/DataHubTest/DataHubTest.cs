using DataHub;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSystem : TestDataHub.System {
  public override void Handle(DataHub<TestData>.Event eve) {
    if (eve is TestEvent te) {
      if (hub.TryGetData(te.ptr, out var data)) {
        data.val = te.val;
        data.UpdateVersion();
        hub.SetData(te.ptr, data);
      }
    }
  }

}


public class TestEvent : TestDataHub.Event {
  public int ptr;
  public int val;

  public TestEvent(int ptr, int val) {
    this.ptr = ptr;
    this.val = val;
  }
}


public class DataHubTest : MonoBehaviour {
  public GameObject binderObj;
  private int ptr;

  void Start() {
    ptr = binderObj.GetInstanceID();
    var data = new TestData();
    data.val = 123;
    data.UpdateVersion();
    TestDataHub.Instance.RegisterData(ptr, data);
    var sys = new TestSystem();
    TestDataHub.Instance.AddSystem(sys);
  }

  void Update() {
    if (Input.GetKeyDown(KeyCode.J)) {
      var eve = new TestEvent(ptr, Time.frameCount);
      TestDataHub.Instance.PushEvent(eve);
    }
  }
}
