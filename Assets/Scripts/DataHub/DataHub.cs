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

    public bool TryGetData(int ptr, out TData res) {
      return (core).TryGetData(ptr, out res);
    }

    public void RegisterData(int ptr, TData data) {
      OnBeforeRegisterData(ptr, ref data);
      core.RegisterData(ptr, data);
    }

    public void DeleteData(int ptr) {
      core.DeleteData(ptr);
    }

    public void SetData(int ptr, TData data) {
      OnBeforeSetData(ptr, ref data);
      core.SetData(ptr, data);
    }

    protected virtual void OnBeforeRegisterData(int ptr,ref TData data) {

    }

    protected virtual void OnBeforeSetData(int ptr,ref TData data) {

    }
  }
}