using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerMouseInteract : MonoBehaviour, IOnLeftMouseUp, IOnMousePureHover, IOnMouseExit, DraggableObject.IOnDragHandler {
  [SerializeField]
  private Color placeableColor;
  [SerializeField]
  private Color unplaceableColor;
  private Outline outline;
  private Collider draggableCollider;
  private DraggingMountManager.DraggablePositionHandler positionHandler;
  private DraggableObject draggableObject;
  private void Awake() {
    outline = GetComponent<Outline>();
    draggableCollider = GetComponent<Collider>();
    draggableObject = GetComponent<DraggableObject>();
  }
  private void Update() {
    if (positionHandler == null) {
      positionHandler = draggableObject.GetPositionHandler() as DraggingMountManager.DraggablePositionHandler;
    }
  }

  public void OnDrag() {
    outline.enabled = true;
    outline.OutlineColor = positionHandler.isPlaceable ? placeableColor : unplaceableColor;
    SetDraggableColliderActive(false);
  }

  public void OnDragEnd() {
    outline.enabled = false;
    outline.OutlineColor = Color.white;
    SetDraggableColliderActive(true);
  }

  public MouseResult OnMousePureHover(MouseInputArgument arg) {
    outline.enabled = true;
    return MouseResult.BreakBehind;
  }

  public void SetDraggableColliderActive(bool active) {
    if (draggableCollider != null) {
      draggableCollider.enabled = active;
    }
  }

  public MouseResult OnLeftMouseUp(MouseInputArgument arg) {
    var ptr = this.FindDataPtr();
    if (TowerDataHub.Instance.TryGetData(ptr, out var data)) {
      InfoPanelView.Instance.SetShow(true);
      InfoPanelView.Instance.ShowViewObject(InfoPanelView.Object.TowerInfo);
      TowerInfoView.Instance.Render(data);
    }
    return MouseResult.BreakBehind;
  }

  public MouseResult OnMouseExiting(MouseInputArgument arg) {
    outline.enabled = false;
    return MouseResult.None;
  }
}
