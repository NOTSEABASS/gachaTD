using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class SwitchTween
{
    private Tween tweenSlot;
    private Dictionary<string, Tween> tweenDict;

    public void SwitchToTween(Tween tween)
    {
        if (tweenSlot != null)
        {
            tweenSlot.Kill();
        }

        tweenSlot = tween;
        tweenSlot.Play();
    }
    
    public void SwitchToTween(string tweenName)
    {
        if (tweenDict.ContainsKey(tweenName))
        {
            return;
        }
        SwitchToTween(tweenDict[tweenName]);
    }

    
}
