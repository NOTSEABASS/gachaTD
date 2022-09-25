using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInput : MonoSingleton<MouseInput>, IMouseInputHandler
{
    private const int maxRayDistance = 1000;
    public enum MouseState
    {
        MouseDown,
        MousePress,
        MouseUp,
        MouseHover
    }

    private MouseState leftState;
    private MouseState rightState;
    private List<IMouseInputHandler> executingDispachers = new List<IMouseInputHandler>();

    void Start()
    {
        leftState = MouseState.MouseHover;
        rightState = MouseState.MouseHover;
    }

    void Update()
    {
        UpdateState(ref leftState, 0);
        UpdateState(ref rightState, 1);

        Execute();
    }

    private void UpdateState(ref MouseState state, int button)
    {
        if (Input.GetMouseButtonDown(button))
        {
            state = MouseState.MouseDown;
        }
        else if (Input.GetMouseButtonUp(button))
        {
            state = MouseState.MouseUp;
        }
        else if (Input.GetMouseButton(button))
        {
            state = MouseState.MousePress;
        }
        else
        {
            state = MouseState.MouseHover;
        }
    }

    private void Execute()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hits = Physics.RaycastAll(ray, maxRayDistance);
        System.Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));
        var dispachers = new List<IMouseInputHandler>();
        foreach (var hit in hits)
        {
            if (hit.transform.TryGetComponent(out MouseInputDispacher dispacher))
            {
                if (dispacher.isEnabled)
                {
                    dispachers.Add(dispacher);
                }
            }
        }

        if (executingDispachers.Count > 0)
        {
            print(executingDispachers.Count);
        }

        var arg = new MouseInputArgument(dispachers, leftState, rightState);

        for (int i = 0; i < executingDispachers.Count; i++)
        {
            var result = executingDispachers[i].OnMouseExecuting(arg);
            if (IsBreakBehind(result))
            {
                return;
            }
            else if (IsDone(result))
            {
                executingDispachers.RemoveAt(i);
                i--;
            }
        }

        if (dispachers.Count == 0)
        {
            return;
        }

        switch (leftState)
        {
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

        switch (rightState)
        {
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

        if (leftState == MouseState.MouseHover)
        {
            OnMouseHover(arg);
        }
    }

    public MouseInputHandleResult OnLeftMouseDown(MouseInputArgument arg)
    {
        foreach (var receiver in arg.handlers)
        {
            var result = receiver.OnLeftMouseDown(arg);
            if (IsBreakBehind(result))
            {
                break;
            }
            else if (IsExecuting(result))
            {
                executingDispachers.Add(receiver);
            }
        }
        return 0;
    }

    public MouseInputHandleResult OnLeftMousePress(MouseInputArgument arg)
    {
        foreach (var receiver in arg.handlers)
        {
            var result = receiver.OnLeftMousePress(arg);
            if (IsBreakBehind(result))
            {
                break;
            }
            else if (IsExecuting(result))
            {
                executingDispachers.Add(receiver);
            }
        }
        return 0;
    }

    public MouseInputHandleResult OnLeftMouseUp(MouseInputArgument arg)
    {
        foreach (var receiver in arg.handlers)
        {
            var result = receiver.OnLeftMouseUp(arg);
            if (IsBreakBehind(result))
            {
                break;
            }
            else if (IsExecuting(result))
            {
                executingDispachers.Add(receiver);
            }
        }
        return 0;
    }

    public MouseInputHandleResult OnRightMouseDown(MouseInputArgument arg)
    {
        foreach (var receiver in arg.handlers)
        {
            var result = receiver.OnRightMouseDown(arg);
            if (IsBreakBehind(result))
            {
                break;
            }
            else if (IsExecuting(result))
            {
                executingDispachers.Add(receiver);
            }
        }
        return 0;
    }

    public MouseInputHandleResult OnRightMousePress(MouseInputArgument arg)
    {
        foreach (var receiver in arg.handlers)
        {
            var result = receiver.OnRightMousePress(arg);
            if (IsBreakBehind(result))
            {
                break;
            }
            else if (IsExecuting(result))
            {
                executingDispachers.Add(receiver);
            }
        }
        return 0;
    }

    public MouseInputHandleResult OnRightMouseUp(MouseInputArgument arg)
    {
        foreach (var receiver in arg.handlers)
        {
            var result = receiver.OnRightMouseUp(arg);
            if (IsBreakBehind(result))
            {
                break;
            }
            else if (IsExecuting(result))
            {
                executingDispachers.Add(receiver);
            }
        }
        return 0;
    }

    public MouseInputHandleResult OnMouseHover(MouseInputArgument arg)
    {
        foreach (var receiver in arg.handlers)
        {
            var result = receiver.OnMouseHover(arg);
            if (IsBreakBehind(result))
            {
                break;
            }
            else if (IsExecuting(result))
            {
                executingDispachers.Add(receiver);
            }
        }
        return 0;
    }

    public static bool IsBreakThis(MouseInputHandleResult result)
    {
        return (result & MouseInputHandleResult.BreakThis) > 0;
    }

    public static bool IsBreakBehind(MouseInputHandleResult result)
    {
        return (result & MouseInputHandleResult.BreakBehind) > 0;
    }

    public static bool IsExecuting(MouseInputHandleResult result)
    {
        return (result & MouseInputHandleResult.Executing) > 0;
    }

    public static bool IsDone(MouseInputHandleResult result)
    {
        return !IsExecuting(result);
    }

}
