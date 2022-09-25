using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableObject : MonoBehaviour, IMouseInputHandler
{
    private bool isDragging = false;
    private 

    void Start()
    {
        
    }

    void Update()
    {
        if (!isDragging)
        {
            return;
        }
        
    }

    private void Place()
    {
        print("Place");
    }

    public MouseInputHandleResult OnLeftMouseDown(MouseInputArgument arg)
    {
        isDragging = true;
        return MouseInputHandleResult.Executing;
    }

    public MouseInputHandleResult OnMouseExecuting(MouseInputArgument arg)
    {
        if (arg.leftState == MouseInput.MouseState.MouseUp)
        {
            isDragging = false;
            Place();
            return MouseInputHandleResult.Done;
        }
        return MouseInputHandleResult.Executing;
    }

}
