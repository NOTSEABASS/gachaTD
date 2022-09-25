using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverReceiverTest : MouseInputHandlerBase {
  private bool lastIsHovering = false;
  private bool isHovering = false;

  private SwitchTween switchTween;

  void Start() {
    switchTween = new SwitchTween();
    switchTween.RegisterTween("flowTilt", () => transform.FlowAndTilt(0.5f));
    switchTween.RegisterTween("downTilt", () => transform.DownAndTilt(0.5f));
  }

  void Update() {
    if (isHovering && !lastIsHovering) {
      switchTween.SwitchToTween("flowTilt");
    }
    if (!isHovering && lastIsHovering) {
      switchTween.SwitchToTween("downTilt");
    }
    lastIsHovering = isHovering;
    isHovering = false;
  }

  public MouseResult OnMouseHover(MouseInputArgument arg) {
    isHovering = true;
    return 0;
  }

  public bool Test() {
    return true;
  }
}
