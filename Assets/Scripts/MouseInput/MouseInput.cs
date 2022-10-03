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
    arg.mousePosition = Input.mousePosition;

    for (int i = 0; i < executings.Count; i++) {
      var result = executings[i].OnMouseExecuting(arg);
      // executing handlers should not break other executing
      // it might cause troubles
      if (!result.HasFlag(MouseResult.Executing)) {
        executings.RemoveAt(i);
        i--;
      }
    }

    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    var hits = Physics.RaycastAll(ray, maxRayDistance);
    Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));

    foreach (var hit in hits) {
      var res = CollectInterfacesAndInvoke(hit.transform, arg);
      if (res.HasFlag(MouseResult.BreakBehind)) {
        break;
      }
    }
  }

  private MouseResult CollectInterfacesAndInvoke(Transform transform, MouseInputArgument arg) {
    MouseResult res = MouseResult.None;
    // warning: 鉴于目前单个物体上多个handler的result都是可以通过或逻辑组合的，因此使用|=
    //          如果mouse result有新内容，要注意是否满足这个约定
    res |= CollectAndInvoke<IOnMouseHover>(transform, arg);

    switch (leftState) {
      case MouseState.MouseDown:
        res |= CollectAndInvoke<IOnLeftMouseDown>(transform, arg);
        break;
      case MouseState.MousePress:
        res |= CollectAndInvoke<IOnLeftMousePress>(transform, arg);
        break;
      case MouseState.MouseUp:
        res |= CollectAndInvoke<IOnLeftMouseUp>(transform, arg);
        break;
    }

    switch (rightState) {
      case MouseState.MouseDown:
        res |= CollectAndInvoke<IOnRightMouseDown>(transform, arg);
        break;
      case MouseState.MousePress:
        res |= CollectAndInvoke<IOnRightMousePress>(transform, arg);
        break;
      case MouseState.MouseUp:
        res |= CollectAndInvoke<IOnRightMouseUp>(transform, arg);
        break;
    }

    if (leftState == MouseState.MouseHover) {
      res |= CollectAndInvoke<IOnMousePureHover>(transform, arg);
    }
    return res;
  }


  private MouseResult CollectAndInvoke<T>(Transform transform, MouseInputArgument arg) where T : IMouseInputHandler {
    var leftMouseDowns = transform.GetComponents<T>();
    bool breakAfterThisObject = false;
    foreach (var handler in leftMouseDowns) {
      var res = InvokeOnSpecificInterface<T>(handler, arg);
      if (res.HasFlag(MouseResult.Executing)) {
        if (handler is IOnMouseExecuting me) {
          executings.Add(me);
        }
        else {
          Debug.LogError(handler.GetType());
        }
      }
      if (res.HasFlag(MouseResult.BreakBehind)) {
        breakAfterThisObject = true;
      }
    };
    if (breakAfterThisObject) {
      return MouseResult.BreakBehind;
    }
    return MouseResult.None;
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
    invokers[typeof(IOnMousePureHover)] = (h, arg) => ((IOnMousePureHover)h).OnMousePureHover(arg);
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
