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

  private bool isShow;

  public void SetShow(bool isShow) {
    if (isShow != this.isShow) {
      this.isShow = isShow;
      if (isShow) {
        viewContainer.DOAnchorPosY(viewContainer.rect.height, showTime).Play();
      } else {
        viewContainer.DOAnchorPosY(0, showTime).Play();
      }
    }
  }
}
