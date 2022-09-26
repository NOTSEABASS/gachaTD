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
    RegisterTween("rotate", () =>
    {
      var left = Quaternion.LookRotation(transform.right, Vector3.up);
      return transform.ShakeRotate(left, 0.5f);
    });
    RegisterTween("flow", () => transform.DownToFlow(0.5f));
    RegisterTween("flowTilt", () => transform.FlowAndTilt(0.5f));
    RegisterTween("down", () => transform.FlowToDown(0.5f));
    RegisterTween("downTilt", () => transform.DownAndTilt(0.5f));
  }
}

public class HoverReceiverTest : MouseInputHandlerBase {
  private bool lastIsHovering = false;
  private bool isHovering = false;

  private SwitchTween switchTween;

  void Start() {
    switchTween = new TestCubeTween(transform);
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

  public override MouseResult OnMouseHover(MouseInputArgument arg) {
    isHovering = true;
    return 0;
  }

  public bool Test() {
    return true;
  }
}
