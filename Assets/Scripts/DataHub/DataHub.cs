using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DataHub {
  public interface IData<TData> {
    public bool HasDiff(TData data);
    public void UpdateVersion();
  }

  public abstract partial class DataHub<TData> : MonoBehaviour
  where TData : IData<TData> {
    private DataHubCore core = new DataHubCore();

    private static DataHub<TData> instance;
    public static DataHub<TData> Instance => instance;

    private void Awake() {
      instance = this;
    }

    private void Update() {
      OnResolve();
    }

    public void AddSystem(System system) {
      (core).AddSystem(system);
    }

    protected bool TryGetData(int ptr, out TData res) {
      return (core).TryGetData(ptr, out res);
    }

    public void RegisterData(int ptr, TData data) {
      core.RegisterData(ptr, data);
    }

    protected void DeleteData(int ptr) {
      core.DeleteData(ptr);
    }

    protected void SetData(int ptr, TData data) {
      core.SetData(ptr, data);
    }

    protected void OnResolve() {
      (core).OnResolve();
    }

    public void PushEvent(Event eve) {
      (core).PushEvent(eve);
    }
  }
}