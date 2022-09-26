using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HoverTest : MonoBehaviour
{
    public int testIndex;
    private SwitchTween switchTween;
    private Outline outlineComponent;
    private Vector3 newPos = Vector3.zero;

    private void Awake()
    {
        outlineComponent = GetComponent<Outline>();
        switchTween = new SwitchTween();
        switchTween.RegisterTween("shake", ()=>transform.Shake(0.5f));
        switchTween.RegisterTween("rotate",()=>
        {
            var left = Quaternion.LookRotation(transform.right,Vector3.up);
            return transform.TweenRotate(left, 0.5f);
        });
        switchTween.RegisterTween("flow",()=>transform.DownToFlow(0.5f));
        switchTween.RegisterTween("flowTilt",()=>transform.FlowAndTilt(0.5f));
        switchTween.RegisterTween("down",()=>transform.FlowToDown(0.5f));
        switchTween.RegisterTween("downTilt",()=>transform.DownAndTilt(0.5f));
        switchTween.RegisterTween("throw",()=>transform.ThrowTo(transform.position + newPos,0.5f));
    }

    public void OnMouseEnter()
    {
        switch (testIndex)
        {
            case 0:
                switchTween.SwitchToTween("shake");
                break;
            case 1:
                switchTween.SwitchToTween("rotate");
                break;
            case 2:
                switchTween.SwitchToTween("flow");
                break;
            case 3:
                switchTween.SwitchToTween("flowTilt");
                break;
            case 4:
                newPos = new Vector3(Random.value*4-2, 0, Random.value*4-2);
                switchTween.SwitchToTween("throw");
                break;
        }

        outlineComponent.enabled = true;

    }
    
    public void OnMouseExit()
    {
        switch (testIndex)
        {
            case 2:
                switchTween.SwitchToTween("down");
                break;
            case 3:
                switchTween.SwitchToTween("downTilt");
                break;
        }
        outlineComponent.enabled = false;
    }
}
