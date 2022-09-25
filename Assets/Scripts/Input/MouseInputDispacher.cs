using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputDispacher :  MouseInputHandlerBase {
  public bool isEnabled;
  public List<MouseInputCallback> onLeftMouseDown;
  public List<MouseInputCallback> onLeftMousePress;
  public List<MouseInputCallback> onLeftMouseUp;
  public List<MouseInputCallback> onRightMouseDown;
  public List<MouseInputCallback> onRightMousePress;
  public List<MouseInputCallback> onRightMouseUp;
  public List<MouseInputCallback> onMouseHover;
  public List<MouseInputCallback> onMouseExecuting;

  private void Start() {
    isEnabled = true;
  }

  private void Update() {

  }

  public override MouseResult OnLeftMouseDown(MouseInputArgument arg) {
    return Dispatch(arg, onLeftMouseDown);
  }

  public override MouseResult OnLeftMousePress(MouseInputArgument arg) {
    return Dispatch(arg, onLeftMousePress);
  }

  public override MouseResult OnLeftMouseUp(MouseInputArgument arg) {
    return Dispatch(arg, onLeftMouseUp);
  }

  public override MouseResult OnRightMouseDown(MouseInputArgument arg) {
    return Dispatch(arg, onRightMouseDown);
  }

  public override MouseResult OnRightMousePress(MouseInputArgument arg) {
    return Dispatch(arg, onRightMousePress);
  }

  public override MouseResult OnRightMouseUp(MouseInputArgument arg) {
    return Dispatch(arg, onRightMouseUp);
  }

  public override MouseResult OnMouseHover(MouseInputArgument arg) {
    return Dispatch(arg, onMouseHover);
  }

  public override MouseResult OnMouseExecuting(MouseInputArgument arg) {
    return Dispatch(arg, onMouseExecuting);
  }

  private MouseResult Dispatch(MouseInputArgument arg, List<MouseInputCallback> callbacks) {
    bool isExecuting = false;
    bool isBreakBehind = false;
    MouseResult r = 0;
    foreach (var receiver in callbacks) {
      var result = receiver.Invoke(arg);
      if (result.HasFlag(MouseResult.Executing)) {
        isExecuting = true;
      }
      if (result.HasFlag(MouseResult.BreakBehind)) {
        isBreakBehind = true;
      }
      else {
        // do nothing
      }
    }
    if (isBreakBehind) {
      r |= MouseResult.BreakBehind;
    }
    if (isExecuting) {
      r |= MouseResult.Executing;
    }
    return r;
  }
}

[Serializable]
public class MouseInputCallback : SerializableCallback<MouseInputArgument, MouseResult> { }


[Serializable]
public class MouseInputArgument : UnityEngine.Object {
  [SerializeField]
  public List<MouseInputHandlerBase> handlers;
  public MouseInput.MouseState leftState;
  public MouseInput.MouseState rightState;

  public MouseInputArgument(List<MouseInputHandlerBase> handlers,
      MouseInput.MouseState leftState, MouseInput.MouseState rightState) {
    this.handlers = handlers;
    this.leftState = leftState;
    this.rightState = rightState;
  }

}
