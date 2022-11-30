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

  protected override void Awake() {
    base.Awake();
    print("5678");
  }

  public void Render(TowerData data) {
    towerBarView.Render(data);
    propertyGroupView.Render(data);

    if (ResourcesLoader.Instance.TryGetTowerResources(data.name, out var resources)) {
      towerNameText.text = resources.displayName;
    }
  }

}
