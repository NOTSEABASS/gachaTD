using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputDispacher : MonoBehaviour, IMouseInputHandler
{
    public bool isEnabled;
    public List<MouseInputCallback> onLeftMouseDown;
    public List<MouseInputCallback> onLeftMousePress;
    public List<MouseInputCallback> onLeftMouseUp;
    public List<MouseInputCallback> onRightMouseDown;
    public List<MouseInputCallback> onRightMousePress;
    public List<MouseInputCallback> onRightMouseUp;
    public List<MouseInputCallback> onMouseHover;
    public List<MouseInputCallback> onMouseExecuting;

    private void Start()
    {
        isEnabled = true;
    }

    private void Update()
    {

    }

    public MouseInputHandleResult OnLeftMouseDown(MouseInputArgument arg)
    {
        return Dispatch(arg, onLeftMouseDown);
    }

    public MouseInputHandleResult OnLeftMousePress(MouseInputArgument arg)
    {
        return Dispatch(arg, onLeftMousePress);
    }

    public MouseInputHandleResult OnLeftMouseUp(MouseInputArgument arg)
    {
        return Dispatch(arg, onLeftMouseUp);
    }

    public MouseInputHandleResult OnRightMouseDown(MouseInputArgument arg)
    {
        return Dispatch(arg, onRightMouseDown);
    }

    public MouseInputHandleResult OnRightMousePress(MouseInputArgument arg)
    {
        return Dispatch(arg, onRightMousePress);
    }

    public MouseInputHandleResult OnRightMouseUp(MouseInputArgument arg)
    {
        return Dispatch(arg, onRightMouseUp);
    }

    public MouseInputHandleResult OnMouseHover(MouseInputArgument arg)
    {
        return Dispatch(arg, onMouseHover);
    }

    public MouseInputHandleResult OnMouseExecuting(MouseInputArgument arg)
    {
        return Dispatch(arg, onMouseExecuting);
    }

    private MouseInputHandleResult Dispatch(MouseInputArgument arg, List<MouseInputCallback> callbacks)
    {
        bool isExecuting = false;
        bool isBreakBehind = false;
        MouseInputHandleResult r = 0;
        foreach (var receiver in callbacks)
        {
            var result = receiver.Invoke(arg);
            if (MouseInput.IsExecuting(result))
            {
                isExecuting = true;
            }
            if (MouseInput.IsBreakBehind(result))
            {
                isBreakBehind = true;
            }
            if (MouseInput.IsBreakThis(result))
            {
                break;
            }
        }
        if (isBreakBehind)
        {
            r |= MouseInputHandleResult.BreakBehind;
        }
        if (isExecuting)
        {
            r |= MouseInputHandleResult.Executing;
        }
        return r;
    }
}

[Serializable]
public class MouseInputCallback : SerializableCallback<MouseInputArgument, MouseInputHandleResult> { }


[Serializable]
public class MouseInputArgument : UnityEngine.Object
{
    [SerializeField]
    public List<IMouseInputHandler> handlers;
    public MouseInput.MouseState leftState;
    public MouseInput.MouseState rightState;

    public MouseInputArgument(List<IMouseInputHandler> handlers, 
        MouseInput.MouseState leftState, MouseInput.MouseState rightState)
    {
        this.handlers = handlers;
        this.leftState = leftState;
        this.rightState = rightState;
    }
    
}
