using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DataHub;

namespace DataHub {

  public partial class DataHub<TData> {
    public class DataHubCore {

      private Dictionary<int, TData> datas = new Dictionary<int, TData>();

      public IEnumerable<TData> allDatas => datas.Values;

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
    }

    public abstract class Binder : MonoBehaviour {
      private int ptr;

      private TData cachedData;
      protected TData Data => cachedData;
      protected int DataPtr => ptr;

      protected virtual void Awake() {
        CheckLegal();
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

      private void CheckLegal() {
        if (gameObject.tag != TagConsts.Data) {
          Debug.LogError("[DataHub.Binder] Binder's gameObject is not tagged \"Data\"");
        }
      }
    }
  }
}
