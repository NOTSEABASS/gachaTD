using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerInfoView : MonoSingleton<TowerInfoView> {
  [SerializeField]
  private RectTransform viewContainer;
  [SerializeField]
  private float showTime;
  [SerializeField]
  private TowerBarView towerBarView;
  [SerializeField]
  private TowerPropertyGroupView propertyGroupView;

  private bool isShow;

  public void SetShow(bool isShow) {
    print(isShow);
    if (isShow != this.isShow) {
      this.isShow = isShow;
      if (isShow) {
        viewContainer.DOAnchorPosY(viewContainer.rect.height, showTime).Play();
      } else {
        viewContainer.DOAnchorPosY(0, showTime).Play();
      }
    }
  }

  public void Render(TowerData data) {
    towerBarView.Render(data);
    propertyGroupView.Render(data);
  }


}
