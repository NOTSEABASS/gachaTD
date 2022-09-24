using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputReceiver
{
    bool OnLeftMouseDown(List<IInputReceiver> receivers) { return false; }
    bool OnLeftMousePress(List<IInputReceiver> receivers) { return false; }
    bool OnLeftMouseUp(List<IInputReceiver> receivers) { return false; }

    bool OnRightMouseDown(List<IInputReceiver> receivers) { return false; }
    bool OnRightMousePress(List<IInputReceiver> receivers) { return false; }
    bool OnRightMouseUp(List<IInputReceiver> receivers) { return false; }

    bool OnMouseHover(List<IInputReceiver> receivers) { return false; }
}
