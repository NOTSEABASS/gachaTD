using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputReceiverPrintTest : MonoBehaviour, IInputReceiver
{
    public bool OnLeftMouseDown(List<IInputReceiver> receivers)
    {
        print(gameObject.name + " OnLeftMouseDown");
        return false;
    }

    public bool OnLeftMousePress(List<IInputReceiver> receivers)
    {
        print(gameObject.name + " OnLeftMousePress");
        return false;
    }

    public bool OnLeftMouseUp(List<IInputReceiver> receivers)
    {
        print(gameObject.name + " OnLeftMouseUp");
        return false;
    }

    public bool OnMouseHover(List<IInputReceiver> receivers)
    {
        print(gameObject.name + " OnMouseHover");
        return true;
    }

    public bool OnRightMouseDown(List<IInputReceiver> receivers)
    {
        print("OnRightMouseDown");
        return false;
    }

    public bool OnRightMousePress(List<IInputReceiver> receivers)
    {
        print("OnRightMousePress");
        return false;
    }

    public bool OnRightMouseUp(List<IInputReceiver> receivers)
    {
        print("OnRightMouseUp");
        return false;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
