using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using UnityEngine;
using UnityEngine.Events;


public class EnemyDataBinder : EnemyDataHub.Binder, IPoolCallback {
  [SerializeField]
  private EnemyTemplateId dataTemplate;
  [SerializeField]
  private UnityEvent<EnemyData> onDataChange;

  private bool isRegistered; //是否注册了数据，一个对象的生命周期内只注册一次
  private bool isDataLoaded; //是否读取了数据，被对象池复用时需要读取多次数据

  protected override void OnDataUpdate(EnemyData data) {
    onDataChange.Invoke(Data);
  }

  protected override void Update() {
    base.Update();
    InitDataIfNot();
  }

  private void InitDataIfNot() {
    if (!isDataLoaded) {
      if (EnemyDataLoader.Instance == null || EnemyDataHub.Instance == null) {
        Debug.Log("[InitDataIfNot] null singleton");
        return;
      }

      var data = EnemyDataLoader.Instance.LoadData(dataTemplate);
      data.gameObject = gameObject;
      data.UpdateVersion();
      if (!isRegistered) {
        EnemyDataHub.Instance.RegisterData(DataPtr, data);
        isRegistered = true;
      } else {
        EnemyDataHub.Instance.SetData(DataPtr, data);
      }
      isDataLoaded = true;
    }

  }

  public void OnGet() {
    isDataLoaded = false;
    InitDataIfNot();
  }
}
