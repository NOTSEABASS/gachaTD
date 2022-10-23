using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TowerDataBinder : TowerDataHub.Binder {
  [SerializeField]
  private TowerDataTemplateId dataTemplate;
  [SerializeField]
  private UnityEvent<TowerData> onDataChange;

  private bool isInited;

  protected override void OnDataUpdate(TowerData data) {
    onDataChange.Invoke(Data);
  }

  protected override void Update() {
    base.Update();
    InitDataIfNot();
  }

  private void InitDataIfNot() {
    if (!isInited) {
      if (TowerDataLoader.Instance != null && TowerDataHub.Instance != null) {
        var data = TowerDataLoader.Instance.LoadData(dataTemplate);
        data.UpdateVersion();
        TowerDataHub.Instance.RegisterData(DataPtr, data);
        isInited = true;
      }
    }
  }

}
