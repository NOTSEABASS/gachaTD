using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Flags]
public enum MouseResult {
  None = 0,
  BreakBehind = 1 << 0,
  Executing = 1 << 1,
}

public abstract class MouseInputHandlerBase : MonoBehaviour {
  public virtual MouseResult OnLeftMouseDown(MouseInputArgument arg) { return 0; }
  public virtual MouseResult OnLeftMousePress(MouseInputArgument arg) { return 0; }
  public virtual MouseResult OnLeftMouseUp(MouseInputArgument arg) { return 0; }
  public virtual MouseResult OnRightMouseDown(MouseInputArgument arg) { return 0; }
  public virtual MouseResult OnRightMousePress(MouseInputArgument arg) { return 0; }
  public virtual MouseResult OnRightMouseUp(MouseInputArgument arg) { return 0; }
  public virtual MouseResult OnMouseHover(MouseInputArgument arg) { return 0; }
  public virtual MouseResult OnMouseExecuting(MouseInputArgument arg) { return 0; }
}
