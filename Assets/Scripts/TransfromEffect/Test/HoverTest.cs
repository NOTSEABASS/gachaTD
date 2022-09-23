using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverTest : MonoBehaviour
{
    public int testIndex;
    public void OnMouseEnter()
    {
        switch (testIndex)
        {
            case 0:
            transform.Shake(0.5f);
                break;
            case 1:
                var left = Quaternion.LookRotation(transform.right,Vector3.up);
                transform.ShakeRotate(left,0.8f);
                break;
            case 2:
                transform.DownToFlow(0.5f);
                break;
            case 3:
                Debug.Log("Enter");
                transform.FlowAndTilt(0.5f);
                break;
        }
       
        
    }

    public void OnMouseExit()
    {
        switch (testIndex)
        {
            case 2:
                transform.FlowToDown(0.5f);
                break;
            case 3:
                transform.DownAndTilt(0.5f);
                break;
        }
    }
}
