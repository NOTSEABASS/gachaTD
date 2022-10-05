using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableObjectPatch : MonoBehaviour, IOnMousePureHover, DraggableObject.IOnDragHandler {
  private Outline outline;
  private Collider draggableCollider;

  private bool outlineEnableInCurrentFrame;

  private void Awake() {
    outline = GetComponent<Outline>();
    draggableCollider = GetComponent<Collider>();
  }

  private void LateUpdate() {
    if (outlineEnableInCurrentFrame) {
      outlineEnableInCurrentFrame = false;
      return;
    }
    outline.enabled = false;
  }

  public void OnDrag() {
    RefreshAllOutlineInChildren();
    SetAllColliderInChildren(false);
  }

  public void OnDragEnd() {
    SetAllColliderInChildren(true);
  }

  public MouseResult OnMousePureHover(MouseInputArgument arg) {
    RefreshAllOutlineInChildren();
    return MouseResult.BreakBehind;
  }

  public void RefreshAllOutlineInChildren() {
    var patches = GetComponentsInChildren<DraggableObjectPatch>();
    foreach (var patch in patches) {
      patch.RefreshOutline();
    }
  }

  public void SetAllColliderInChildren(bool active) {
    var patches = GetComponentsInChildren<DraggableObjectPatch>();
    foreach (var patch in patches) {
      patch.SetDraggableColliderActive(active);
    }
  }

  public void RefreshOutline() {
    outline.enabled = true;
    outlineEnableInCurrentFrame = true;
  }

  public void SetDraggableColliderActive(bool active) {
    if (draggableCollider != null) {
      draggableCollider.enabled = active;
    }
  }


}
