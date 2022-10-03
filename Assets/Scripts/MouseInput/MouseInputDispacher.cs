using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputDispacher : MonoBehaviour {
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
  public Vector2 mousePosition;
  [SerializeField]
  public MouseInput.MouseState leftState;
  public MouseInput.MouseState rightState;

  public MouseInputArgument(MouseInput.MouseState leftState, MouseInput.MouseState rightState) {
    this.leftState = leftState;
    this.rightState = rightState;
  }

}
