using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DataHub;

namespace DataHub {

  public partial class DataHub<TData> {


    public abstract class Event {

    }

    public abstract class System {
      protected DataHubCore hub;
      public void SetDataHub(DataHubCore hub) {
        this.hub = hub;
      }

      public abstract void Handle(Event eve);
    }

    public class DataHubCore {

      private Dictionary<int, TData> datas = new Dictionary<int, TData>();
      private List<System> systems = new List<System>();
      private Queue<Event> eventBuffer = new Queue<Event>();

      public void PushEvent(Event eve) {
        eventBuffer.Enqueue(eve);
      }

      public void AddSystem(System system) {
        system.SetDataHub(this);
        systems.Add(system);
      }

      public bool TryGetData(int ptr, out TData res) {
        return datas.TryGetValue(ptr, out res);
      }
      public void RegisterData(int ptr, TData data) {
        Debug.Assert(!datas.ContainsKey(ptr));
        datas[ptr] = data;
      }

      public void SetData(int ptr, TData data) {
        Debug.Assert(datas.ContainsKey(ptr));
        datas[ptr] = data;
      }

      public void DeleteData(int ptr) {
        datas.Remove(ptr);
      }
      public void OnResolve() {
        while (eventBuffer.Count > 0) {
          var eve = eventBuffer.Dequeue();
          foreach (var sys in systems) {
            sys.Handle(eve);
          }
        }
      }
    }

    public abstract class Binder : MonoBehaviour {
      private int ptr;

      private TData cachedData;
      protected TData Data => cachedData;
      protected int DataPtr => ptr;
      protected virtual void Awake() {
        ptr = gameObject.GetInstanceID();
      }

      protected virtual void Update() {
        CheckDataUpdate();
      }

      protected void CheckDataUpdate() {
        if (Instance != null && Instance.TryGetData(ptr, out TData data)) {
          if (data.HasDiff(cachedData)) {
            cachedData = data;
            OnDataUpdate(data);
          }
        }
      }

      protected virtual void OnDataUpdate(TData data) {

      }
    }
  }
}
