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


public interface IOnLeftMouseDown : MouseInput.IMouseInputHandler {
  public MouseResult OnLeftMouseDown(MouseInputArgument arg);

}

public interface IOnLeftMousePress : MouseInput.IMouseInputHandler {
  public MouseResult OnLeftMousePress(MouseInputArgument arg);

}


public interface IOnLeftMouseUp : MouseInput.IMouseInputHandler {
  public MouseResult OnLeftMouseUp(MouseInputArgument arg);

}

public interface IOnRightMouseDown : MouseInput.IMouseInputHandler {
  public MouseResult OnRightMouseDown(MouseInputArgument arg);

}

public interface IOnRightMousePress : MouseInput.IMouseInputHandler {
  public MouseResult OnRightMousePress(MouseInputArgument arg);

}
public interface IOnRightMouseUp : MouseInput.IMouseInputHandler {
  public MouseResult OnRightMouseUp(MouseInputArgument arg);

}

//invoke as long as the mouse position is on object
public interface IOnMouseHover : MouseInput.IMouseInputHandler {
  public MouseResult OnMouseHover(MouseInputArgument arg);

}

//invoke when mouse position is on object, and is not down/press/up
public interface IOnMousePureHover : MouseInput.IMouseInputHandler {
  public MouseResult OnMousePureHover(MouseInputArgument arg);
}

public interface IOnMouseExecuting : MouseInput.IMouseInputHandler {
  public MouseResult OnMouseExecuting(MouseInputArgument arg);

}

public interface IOnMouseDrag : MouseInput.IMouseInputHandler {
  public MouseResult OnMouseStartDrag(MouseInputArgument arg);
}