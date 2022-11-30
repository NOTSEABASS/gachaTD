using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPanelView : MonoSingleton<InfoPanelView>, ClickFallback.IListener {


  public enum Object {
    TowerInfo,
    EnemyInfo
  }

  [SerializeField]
  private RectTransform viewContainer;
  [SerializeField]
  private float showTime;

  [SerializeField]
  private SerializeDict<Object, GameObject> viewObjects = new SerializeDict<Object, GameObject>();

  private bool isShow;

  private GameObject currentViewObject;

  protected override void Awake() {
    base.Awake();
    viewObjects.Construct();
  }

  public void ShowViewObject(Object obj) {  
    var nextViewObject = viewObjects[obj];
    if (currentViewObject != null) {
      currentViewObject.SetActive(false);
    }
    if (nextViewObject != null) { 
      nextViewObject.SetActive(true);  
    }

    currentViewObject = nextViewObject;
  }

  public void SetShow(bool isShow) {
    if (isShow != this.isShow) {
      this.isShow = isShow;
      if (isShow) {
        viewContainer.DOAnchorPosY(viewContainer.rect.height, showTime).Play();
        ClickFallback.Instance.AddListener(this);
      } else {
        viewContainer.DOAnchorPosY(0, showTime).Play();
      }
    }
  }
  public ClickFallback.Result OnClickFallback(MouseInputArgument arg) {
    if (arg.leftState == MouseInput.State.Up || arg.rightState == MouseInput.State.Up) {
      SetShow(false);
      return ClickFallback.Result.StopListen;
    }
    return ClickFallback.Result.None;
  }
}
