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
        }
       
        
    }
}
