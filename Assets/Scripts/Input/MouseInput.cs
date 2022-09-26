using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Object = UnityEngine.Object;

public class MouseInput : MonoSingleton<MouseInput> {
  public enum MouseState {
    MouseDown,
    MousePress,
    MouseUp,
    MouseHover
  }

  private const int maxRayDistance = 1000;

  private MouseState leftState;
  private MouseState rightState;
  private List<MouseInputHandlerBase> executingDispachers = new List<MouseInputHandlerBase>();

  void Start() {
    leftState = MouseState.MouseHover;
    rightState = MouseState.MouseHover;
  }

  void Update() {
    UpdateState(ref leftState, 0);
    UpdateState(ref rightState, 1);

    Execute();
  }

  private void UpdateState(ref MouseState state, int button) {
    if (Input.GetMouseButtonDown(button)) {
      state = MouseState.MouseDown;
    }
    else if (Input.GetMouseButtonUp(button)) {
      state = MouseState.MouseUp;
    }
    else if (Input.GetMouseButton(button)) {
      state = MouseState.MousePress;
    }
    else {
      state = MouseState.MouseHover;
    }
  }

  private void Execute() {
    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    var hits = Physics.RaycastAll(ray, maxRayDistance);
    Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));
    var handlers = new List<MouseInputHandlerBase>();
    foreach (var hit in hits) {
      MouseInputDispacher dispacher = null;
      MouseInputHandlerBase handler = null;
      if (hit.transform.TryGetComponent(out dispacher) ||
          hit.transform.TryGetComponent(out handler)) {
        var handlerNotNull = dispacher ?? handler;
        if (handlerNotNull.enabled) {
          handlers.Add(handlerNotNull);
        }
      }
    }

    if (executingDispachers.Count > 0) {
      print(executingDispachers.Count);
    }

    var arg = new MouseInputArgument(handlers, leftState, rightState);

    for (int i = 0; i < executingDispachers.Count; i++) {
      var result = executingDispachers[i].OnMouseExecuting(arg);
      if (result.HasFlag(MouseResult.BreakBehind)) {
        return;
      }
      else if (!result.HasFlag(MouseResult.Executing)) {
        executingDispachers.RemoveAt(i);
        i--;
      }
    }

    if (handlers.Count == 0) {
      return;
    }

    switch (leftState) {
      case MouseState.MouseDown:
        OnLeftMouseDown(arg);
        break;
      case MouseState.MousePress:
        OnLeftMousePress(arg);
        break;
      case MouseState.MouseUp:
        OnLeftMouseUp(arg);
        break;
    }

    switch (rightState) {
      case MouseState.MouseDown:
        OnRightMouseDown(arg);
        break;
      case MouseState.MousePress:
        OnRightMousePress(arg);
        break;
      case MouseState.MouseUp:
        OnRightMouseUp(arg);
        break;
    }

    if (leftState == MouseState.MouseHover) {
      OnMouseHover(arg);
    }
  }

  public MouseResult OnLeftMouseDown(MouseInputArgument arg) {
    foreach (var receiver in arg.handlers) {
      var result = receiver.OnLeftMouseDown(arg);
      if (result.HasFlag(MouseResult.BreakBehind)) {
        break;
      }
      else if (result.HasFlag(MouseResult.Executing)) {
        executingDispachers.Add(receiver);
      }
    }
    return 0;
  }

  public MouseResult OnLeftMousePress(MouseInputArgument arg) {
    foreach (var receiver in arg.handlers) {
      var result = receiver.OnLeftMousePress(arg);
      if (result.HasFlag(MouseResult.BreakBehind)) {
        break;
      }
      else if (result.HasFlag(MouseResult.Executing)) {
        executingDispachers.Add(receiver);
      }
    }
    return 0;
  }

  public MouseResult OnLeftMouseUp(MouseInputArgument arg) {
    foreach (var receiver in arg.handlers) {
      var result = receiver.OnLeftMouseUp(arg);
      if (result.HasFlag(MouseResult.BreakBehind)) {
        break;
      }
      else if (result.HasFlag(MouseResult.Executing)) {
        executingDispachers.Add(receiver);
      }
    }
    return 0;
  }

  public MouseResult OnRightMouseDown(MouseInputArgument arg) {
    foreach (var receiver in arg.handlers) {
      var result = receiver.OnRightMouseDown(arg);
      if (result.HasFlag(MouseResult.BreakBehind)) {
        break;
      }
      else if (result.HasFlag(MouseResult.Executing)) {
        executingDispachers.Add(receiver);
      }
    }
    return 0;
  }

  public MouseResult OnRightMousePress(MouseInputArgument arg) {
    foreach (var receiver in arg.handlers) {
      var result = receiver.OnRightMousePress(arg);
      if (result.HasFlag(MouseResult.BreakBehind)) {
        break;
      }
      else if (result.HasFlag(MouseResult.Executing)) {
        executingDispachers.Add(receiver);
      }
    }
    return 0;
  }

  public MouseResult OnRightMouseUp(MouseInputArgument arg) {
    foreach (var receiver in arg.handlers) {
      var result = receiver.OnRightMouseUp(arg);

      result.HasFlag(MouseResult.BreakBehind);
      if (result.HasFlag(MouseResult.BreakBehind)) {
        break;
      }
      else if (result.HasFlag(MouseResult.Executing)) {
        executingDispachers.Add(receiver);
      }
    }
    return 0;
  }

  public MouseResult OnMouseHover(MouseInputArgument arg) {
    foreach (var receiver in arg.handlers) {
      var result = receiver.OnMouseHover(arg);
      if (result.HasFlag(MouseResult.BreakBehind)) {
        break;
      }
      else if (result.HasFlag(MouseResult.Executing)) {
        executingDispachers.Add(receiver);
      }
    }
    return 0;
  }

}
