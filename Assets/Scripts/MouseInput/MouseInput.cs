using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Object = UnityEngine.Object;
using UnityEditor.Experimental.GraphView;
using UnityEngine.EventSystems;

[Serializable]
public class MouseInputArgument : UnityEngine.Object {
  public Vector2 mousePosition;
  [SerializeField]
  public MouseInput.State leftState;
  public MouseInput.State rightState;

  public MouseInputArgument(MouseInput.State leftState, MouseInput.State rightState) {
    this.leftState = leftState;
    this.rightState = rightState;
  }

}

public class MouseInput : MonoSingleton<MouseInput> {
  public interface IMouseInputHandler { }

  public enum State {
    Down,
    Press,
    Up,
    Hover
  }

  private const int maxRayDistance = 1000;

  [SerializeField, Min(0)]
  private float dragThreshold;
  private Vector2 dragOriginPosition;

  private State leftState;
  private State rightState;

  private List<IOnMouseDrag> dragHandlersCache = new List<IOnMouseDrag>();
  private List<IOnMouseExecuting> executings = new List<IOnMouseExecuting>();

  private RaycastHit[] rayHitBuffer;

  void Start() {
    leftState = State.Hover;
    rightState = State.Hover;
  }

  void Update() {
    UpdateState(ref leftState, 0);
    UpdateState(ref rightState, 1);

    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    rayHitBuffer = Physics.RaycastAll(ray, maxRayDistance);
    Array.Sort(rayHitBuffer, (x, y) => x.distance.CompareTo(y.distance));

    var arg = new MouseInputArgument(leftState, rightState);
    arg.mousePosition = Input.mousePosition;

    UpdateExcuting(arg);

    if (!EventSystem.current.IsPointerOverGameObject()) {
      ExecuteDragInput(arg);
      ExecuteMouseButtonInput(arg);
    }
  }

  private void UpdateState(ref State state, int button) {
    if (Input.GetMouseButtonDown(button)) {
      state = State.Down;
    } else if (Input.GetMouseButtonUp(button)) {
      state = State.Up;
    } else if (Input.GetMouseButton(button)) {
      state = State.Press;
    } else {
      state = State.Hover;
    }
  }

  private void ExecuteDragInput(MouseInputArgument arg) {
    if (leftState == State.Down) {
      foreach (var hit in rayHitBuffer) {
        dragHandlersCache.AddRange(hit.collider.GetComponentsInChildren<IOnMouseDrag>());
        dragOriginPosition = Input.mousePosition.XY();
      }
    } else if (leftState != State.Press) {
      dragHandlersCache.Clear();
    } else if (leftState == State.Press) {
      var cur = Input.mousePosition;
      if (Vector2.Distance(dragOriginPosition, cur) > dragThreshold) {
        foreach (var dragHandler in dragHandlersCache) {
          var res = dragHandler.OnMouseStartDrag(arg);
          if (res.HasFlag(MouseResult.Executing) &&
              dragHandler is IOnMouseExecuting me) {
            executings.Add(me);
          }
          if (res.HasFlag(MouseResult.BreakBehind)) {
            break;
          }
        }
        dragHandlersCache.Clear();
      }
    }
  }

  private void UpdateExcuting(MouseInputArgument arg) {
    for (int i = 0; i < executings.Count; i++) {
      var result = executings[i].OnMouseExecuting(arg);
      // executing handlers should not break other executing
      // it might cause troubles
      if (!result.HasFlag(MouseResult.Executing)) {
        executings.RemoveAt(i);
        i--;
      }
    }
  }

  private void ExecuteMouseButtonInput(MouseInputArgument arg) {

    foreach (var hit in rayHitBuffer) {
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
      case State.Down:
        res |= CollectAndInvoke<IOnLeftMouseDown>(transform, arg);
        break;
      case State.Press:
        res |= CollectAndInvoke<IOnLeftMousePress>(transform, arg);
        break;
      case State.Up:
        res |= CollectAndInvoke<IOnLeftMouseUp>(transform, arg);
        break;
    }

    switch (rightState) {
      case State.Down:
        res |= CollectAndInvoke<IOnRightMouseDown>(transform, arg);
        break;
      case State.Press:
        res |= CollectAndInvoke<IOnRightMousePress>(transform, arg);
        break;
      case State.Up:
        res |= CollectAndInvoke<IOnRightMouseUp>(transform, arg);
        break;
    }

    if (leftState == State.Hover) {
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
        } else {
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
