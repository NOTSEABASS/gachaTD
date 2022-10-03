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

public interface IMouseInputHandler {

}

public interface IOnLeftMouseDown : IMouseInputHandler {
  public MouseResult OnLeftMouseDown(MouseInputArgument arg);

}

public interface IOnLeftMousePress : IMouseInputHandler {
  public MouseResult OnLeftMousePress(MouseInputArgument arg);

}


public interface IOnLeftMouseUp : IMouseInputHandler {
  public MouseResult OnLeftMouseUp(MouseInputArgument arg);

}

public interface IOnRightMouseDown : IMouseInputHandler {
  public MouseResult OnRightMouseDown(MouseInputArgument arg);

}

public interface IOnRightMousePress : IMouseInputHandler {
  public MouseResult OnRightMousePress(MouseInputArgument arg);

}
public interface IOnRightMouseUp : IMouseInputHandler {
  public MouseResult OnRightMouseUp(MouseInputArgument arg);

}

//invoke as long as the mouse position is on object
public interface IOnMouseHover : IMouseInputHandler {
  public MouseResult OnMouseHover(MouseInputArgument arg);

}

//invoke when mouse position is on object, and is not down/press/up
public interface IOnMousePureHover : IMouseInputHandler {
  public MouseResult OnMousePureHover(MouseInputArgument arg);
}

public interface IOnMouseExecuting : IMouseInputHandler {
  public MouseResult OnMouseExecuting(MouseInputArgument arg);

}
