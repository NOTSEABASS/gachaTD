using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MouseInputHandleResult
{
    ContinueThis = 0b000,
    BreakThis = 0b001,

    ContinueBehind = 0b000,
    BreakBehind = 0b010,

    Done = 0b000,
    Executing = 0b100
}

public interface IMouseInputHandler
{
    MouseInputHandleResult OnLeftMouseDown(MouseInputArgument arg) { return 0; }
    MouseInputHandleResult OnLeftMousePress(MouseInputArgument arg) { return 0; }
    MouseInputHandleResult OnLeftMouseUp(MouseInputArgument arg) { return 0; }

    MouseInputHandleResult OnRightMouseDown(MouseInputArgument arg) { return 0; }
    MouseInputHandleResult OnRightMousePress(MouseInputArgument arg) { return 0; }
    MouseInputHandleResult OnRightMouseUp(MouseInputArgument arg) { return 0; }

    MouseInputHandleResult OnMouseHover(MouseInputArgument arg) { return 0; }

    MouseInputHandleResult OnMouseExecuting(MouseInputArgument arg) { return 0; }
}
