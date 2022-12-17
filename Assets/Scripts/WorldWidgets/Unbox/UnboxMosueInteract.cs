using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class UnboxMosueInteract : MonoBehaviour, IOnMouseExit, IOnMouseHover {
  [AutoProperty, SerializeField]
  private Outline outline;
  [AutoProperty, SerializeField]
  private UnboxController unboxController;
  [SerializeField]
  private float slideSpeed;

  private bool isHolding;
  private bool isHovering;
  private Vector2 lastHoldingPos;

  private void Update() {
    outline.enabled = isHolding || isHovering;
  }

  private void CaculateSlider(Vector2 cur) {
    var deltaVector = cur - lastHoldingPos;
    var slideVector = unboxController.GetSliderVectorInCamera();
    slideVector.z = 0;
    var projection = Vector3.Dot(deltaVector, slideVector.normalized);
    unboxController.MoveSlider(projection * slideSpeed);
    lastHoldingPos = cur;
  }

  public MouseResult OnMouseHover(MouseInputArgument arg) {
    isHovering = true;
    if (arg.leftState != MouseInput.State.Press) {
      isHolding = false;
      return MouseResult.BreakBehind;
    }

    if (!isHolding) {
      lastHoldingPos = arg.mousePosition;
    }
    isHolding = true;

    CaculateSlider(arg.mousePosition);
    return MouseResult.BreakBehind;
  }

  public MouseResult OnMouseExiting(MouseInputArgument arg) {
    isHovering = false;
    if (isHolding) {
      CaculateSlider(arg.mousePosition);
    }
    isHolding = false;
    return MouseResult.None;
  }


}
