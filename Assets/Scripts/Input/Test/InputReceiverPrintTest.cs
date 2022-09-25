using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputReceiverPrintTest : MonoBehaviour, IMouseInputHandler
{
    public MouseInputHandleResult OnLeftMouseDown(MouseInputArgument arg)
    {
        print(gameObject.name + " OnLeftMouseDown");
        return 0;
    }

    public MouseInputHandleResult OnLeftMousePress(MouseInputArgument arg)
    {
        print(gameObject.name + " OnLeftMousePress");
        return 0;
    }

    public MouseInputHandleResult OnLeftMouseUp(MouseInputArgument arg)
    {
        print(gameObject.name + " OnLeftMouseUp");
        return 0;
    }

    public MouseInputHandleResult OnMouseHover(MouseInputArgument arg)
    {
        print(gameObject.name + " OnMouseHover");
        return 0;
    }

    public MouseInputHandleResult OnRightMouseDown(MouseInputArgument arg)
    {
        print("OnRightMouseDown");
        return 0;
    }

    public MouseInputHandleResult OnRightMousePress(MouseInputArgument arg)
    {
        print("OnRightMousePress");
        return 0;
    }

    public MouseInputHandleResult OnRightMouseUp(MouseInputArgument arg)
    {
        print("OnRightMouseUp");
        return 0;
    }

}
