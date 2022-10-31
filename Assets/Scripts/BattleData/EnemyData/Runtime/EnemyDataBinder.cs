using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class EnemyDataBinder : EnemyDataHub.Binder {
  [SerializeField]
  private EnemyTemplateId dataTemplate;
  [SerializeField]
  private UnityEvent<EnemyData> onDataChange;

  private bool isInited;

  protected override void OnDataUpdate(EnemyData data) {
    onDataChange.Invoke(Data);
  }

  protected override void Update() {
    base.Update();
    InitDataIfNot();
  }

  private void InitDataIfNot() {
    if (!isInited) {
      if (EnemyDataLoader.Instance != null && EnemyDataHub.Instance != null) {
        var data = EnemyDataLoader.Instance.LoadData(dataTemplate);
        EnemyDataHub.Instance.RegisterData(DataPtr, data);
        isInited = true;
      }
    }
  }
}
