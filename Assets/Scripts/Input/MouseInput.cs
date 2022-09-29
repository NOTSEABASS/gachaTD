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
  private List<IOnMouseExecuting> executings = new List<IOnMouseExecuting>();

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
    var arg = new MouseInputArgument(leftState, rightState);

    for (int i = 0; i < executings.Count; i++) {
      var result = executings[i].OnMouseExecuting(arg);
      if (result.HasFlag(MouseResult.BreakBehind)) {
        return;
      }
      else if (!result.HasFlag(MouseResult.Executing)) {
        executings.RemoveAt(i);
        i--;
      }
    }

    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    var hits = Physics.RaycastAll(ray, maxRayDistance);
    Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));

    foreach (var hit in hits) {
      CollectInterfacesAndInvoke(hit.transform, arg);
    }
  }

  private void CollectInterfacesAndInvoke(Transform transform, MouseInputArgument arg) {
    switch (leftState) {
      case MouseState.MouseDown:
        CollectAndInvoke<IOnLeftMouseDown>(transform, arg);
        break;
      case MouseState.MousePress:
        CollectAndInvoke<IOnLeftMousePress>(transform, arg);
        break;
      case MouseState.MouseUp:
        CollectAndInvoke<IOnLeftMouseUp>(transform, arg);
        break;
    }

    switch (rightState) {
      case MouseState.MouseDown:
        CollectAndInvoke<IOnRightMouseDown>(transform, arg);
        break;
      case MouseState.MousePress:
        CollectAndInvoke<IOnRightMousePress>(transform, arg);
        break;
      case MouseState.MouseUp:
        CollectAndInvoke<IOnRightMouseUp>(transform, arg);
        break;
    }

    if (leftState == MouseState.MouseHover) {
      CollectAndInvoke<IOnMouseHover>(transform, arg);
    }
  }

  private MouseResult CollectAndInvoke<T>(Transform transform, MouseInputArgument arg) where T : IMouseInputHandler {
    var leftMouseDowns = transform.GetComponents<T>();
    MouseResult res = MouseResult.None;
    foreach (var handler in leftMouseDowns) {
      res = InvokeOnSpecificInterface<T>(handler, arg);
      if (res.HasFlag(MouseResult.Executing)) {
        if (handler is IOnMouseExecuting me) {
          executings.Add(me);
        }
        else {
          Debug.LogError(handler.GetType());
        }
      }
      if (res.HasFlag(MouseResult.BreakBehind)) {
        return res;
      }
    };
    return res;
  }


  private Dictionary<Type, Func<IMouseInputHandler, MouseInputArgument, MouseResult>> invokers = new();
  private bool hasInitInvokers;
  private void InitInvokersIfNot() {
    if (hasInitInvokers) {
      return;
    }
    invokers[typeof(IOnLeftMouseDown)] = (h, arg) => ((IOnLeftMouseDown)h).OnLeftMouseDown(arg);
    invokers[typeof(IOnLeftMousePress)] = (h, arg) => ((IOnLeftMousePress)h).OnLeftMousePress(arg);
    invokers[typeof(IOnLeftMouseUp)] = (h, arg) => ((IOnLeftMouseUp)h).OnLeftMouseUp(arg);
    invokers[typeof(IOnRightMouseDown)] = (h, arg) => ((IOnRightMouseDown)h).OnRightMouseDown(arg);
    invokers[typeof(IOnRightMousePress)] = (h, arg) => ((IOnRightMousePress)h).OnRightMousePress(arg);
    invokers[typeof(IOnRightMouseUp)] = (h, arg) => ((IOnRightMouseUp)h).OnRightMouseUp(arg);
    invokers[typeof(IOnMouseHover)] = (h, arg) => ((IOnMouseHover)h).OnMouseHover(arg);
  }
  private MouseResult InvokeOnSpecificInterface<T>(IMouseInputHandler handler, MouseInputArgument arg) {
    InitInvokersIfNot();
    if (invokers.TryGetValue(typeof(T), out var invoker)) {
      var res = invoker(handler, arg);
      return res;
    }

    Debug.LogError(typeof(T));
    return MouseResult.None;
  }
}
