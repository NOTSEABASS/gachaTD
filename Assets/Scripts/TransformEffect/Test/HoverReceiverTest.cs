using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCubeTween : SwitchTween {

  private Transform transform;
  public TestCubeTween(Transform transform) {
    this.transform = transform;
  }

  protected override void OnInit() {
    RegisterTween("shake", () => transform.Shake(0.5f));
    RegisterTween("rotate", () => {
      var left = Quaternion.LookRotation(transform.right, Vector3.up);
      return transform.ShakeRotate(left, 0.5f);
    });
    RegisterTween("flow", () => transform.DownToFlow(0.5f));
    RegisterTween("flowTilt", () => transform.FlowAndTilt(0.5f));
    RegisterTween("down", () => transform.FlowToDown(0.5f));
    RegisterTween("downTilt", () => transform.DownAndTilt(0.5f));
    RegisterTween("throw", () => transform.ThrowTo(transform.position + new Vector3(1, 0, 0), 0.5f));
  }
}

public class HoverReceiverTest : MonoBehaviour, IOnLeftMouseDown, IOnMouseExecuting, IOnMouseHover {
  private bool lastIsHovering = false;
  private bool isHovering = false;
  private bool isDragging = false;

  private Outline outline;
  private SwitchTween switchTween;

  private void Awake() {
    outline = GetComponent<Outline>();
  }

  void Start() {
    switchTween = new TestCubeTween(transform);
  }

  void Update() {
    if (isHovering && !lastIsHovering) {
      switchTween.SwitchToTween("flowTilt");
      outline.enabled = true;
    }
    if (!isHovering && lastIsHovering) {
      switchTween.SwitchToTween("downTilt");
      if (!isDragging) {
        outline.enabled = false;
      }
    }
    lastIsHovering = isHovering;
    isHovering = false;
  }

  public MouseResult OnMouseHover(MouseInputArgument arg) {
    isHovering = true;
    return 0;
  }

  public MouseResult OnLeftMouseUp(MouseInputArgument arg) {
    isDragging = true;
    outline.enabled = true;
    return MouseResult.Executing;
  }

  public MouseResult OnMouseExecuting(MouseInputArgument arg) {
    if (arg.leftState == MouseInput.State.Up) {
      isDragging = false;
      outline.enabled = false;
      return MouseResult.None;
    }
    return MouseResult.Executing;
  }

  public bool Test() {
    return true;
  }
}
