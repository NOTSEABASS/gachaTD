using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerInfoView : MonoSingleton<TowerInfoView> {
  [SerializeField]
  private TowerBarView towerBarView;
  [SerializeField]
  private TowerPropertyGroupView propertyGroupView;
  [SerializeField]
  private TMP_Text towerNameText;


  private TowerData cachedData;

  private void Update() {
    if (TowerDataHub.Instance == null) {
      return;
    }

    if (TowerDataHub.Instance.TryGetData(cachedData.ptr, out var data)) {
      if (data.isDead) {
        InfoPanelView.Instance.SetShow(false);
      } else if (data.HasDiff(cachedData)) {
        Render(data, true);
        cachedData = data;
      }
    }
  }

  public void Render(TowerData data, bool isRefresh = false) {
    cachedData = data;
    towerBarView.Render(data);
    propertyGroupView.Render(data);

    if (isRefresh) {
      return;
    }

    if (ResourcesLoader.Instance.TryGetTowerResources(data.name, out var resources)) {
      towerNameText.text = resources.displayName;
    }
  }

}
