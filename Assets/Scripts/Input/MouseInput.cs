using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInput : MonoSingleton<MouseInput>, IInputReceiver
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
        } else if (Input.GetMouseButton(button))
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
        var receivers = new List<IInputReceiver>();
        foreach (var hit in hits)
        {
            if (hit.transform.TryGetComponent(out IInputReceiver receiver))
            {
                receivers.Add(receiver);
            }
        }

        switch (leftState)
        {
            case MouseState.MouseDown:
                OnLeftMouseDown(receivers);
                break;
            case MouseState.MousePress:
                OnLeftMousePress(receivers);
                break;
            case MouseState.MouseUp:
                OnLeftMouseUp(receivers);
                break;
        }

        switch (rightState)
        {
            case MouseState.MouseDown:
                OnRightMouseDown(receivers);
                break;
            case MouseState.MousePress:
                OnRightMousePress(receivers);
                break;
            case MouseState.MouseUp:
                OnRightMouseUp(receivers);
                break;
        }

        if (leftState == MouseState.MouseHover)
        {
            OnMouseHover(receivers);
        }
    }

    public bool OnLeftMouseDown(List<IInputReceiver> receivers)
    {
        foreach (var receiver in receivers)
        {
            if (receiver.OnLeftMouseDown(receivers))
            {
                break;
            }
        }
        return true;
    }

    public bool OnLeftMousePress(List<IInputReceiver> receivers)
    {
        foreach (var receiver in receivers)
        {
            if (receiver.OnLeftMousePress(receivers))
            {
                break;
            }
        }
        return true;
    }

    public bool OnLeftMouseUp(List<IInputReceiver> receivers)
    {
        foreach (var receiver in receivers)
        {
            if (receiver.OnLeftMouseUp(receivers))
            {
                break;
            }
        }
        return true;
    }

    public bool OnRightMouseDown(List<IInputReceiver> receivers)
    {
        foreach (var receiver in receivers)
        {
            if (receiver.OnRightMouseDown(receivers))
            {
                break;
            }
        }
        return true;
    }

    public bool OnRightMousePress(List<IInputReceiver> receivers)
    {
        foreach (var receiver in receivers)
        {
            if (receiver.OnRightMousePress(receivers))
            {
                break;
            }
        }
        return true;
    }

    public bool OnRightMouseUp(List<IInputReceiver> receivers)
    {
        foreach (var receiver in receivers)
        {
            if (receiver.OnRightMouseUp(receivers))
            {
                break;
            }
        }
        return true;
    }

    public bool OnMouseHover(List<IInputReceiver> receivers)
    {
        foreach (var receiver in receivers)
        {
            if (receiver.OnMouseHover(receivers))
            {
                break;
            }
        }
        return true;
    }
}
