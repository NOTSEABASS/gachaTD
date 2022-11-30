using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyInfoView : MonoSingleton<EnemyInfoView> {
  [SerializeField]
  private EnemyBarView enemyBarView;
  [SerializeField]
  private EnemyPropertyGroupView propertyGroupView;
  [SerializeField]
  private TMP_Text nameText;

  private EnemyData cachedData;

  protected override void Awake() {
    base.Awake();
  }

  private void Update() {
    if (EnemyDataHub.Instance == null) {
      return;
    }

    if (EnemyDataHub.Instance.TryGetData(cachedData.ptr, out var data)) {
      if (data.isDead) {
        InfoPanelView.Instance.SetShow(false);
      } else if (data.HasDiff(cachedData)) {
        Render(data, true);
        cachedData = data;
      }
    }
  }

  public void Render(EnemyData data, bool isRefresh = false) {
    cachedData = data;

    propertyGroupView.Render(data);
    enemyBarView.Render(data);

    if (isRefresh) {
      return;
    }

    if (ResourcesLoader.Instance.TryGetEnemyResources(data.name, out var resources)) {
      nameText.text = resources.displayName;
      enemyBarView.Render(data);
    }
  }
}
