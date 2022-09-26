using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SwitchTween
{
    private Tween tweenSlot;
    private Dictionary<string, Func<Tween>> tweenDict;

    public SwitchTween()
    {
        tweenDict = new Dictionary<string, Func<Tween>>();
    }

    private void SwitchToTween(Func<Tween> tween)
    {
        if (tweenSlot != null)
        {
            if (tweenSlot.IsActive())
            {
                // 防止动画未播完时下一个动画在当前未完成状态基础上继续播放，优先将动画播完
                tweenSlot.Goto(tweenSlot.Duration());
                tweenSlot.Kill();
            }
        }
        Debug.Log("Play");
        tweenSlot = tween.Invoke();
        tweenSlot.Play();
    }

    public void SwitchToTween(string tweenName)
    {
        Debug.Log("Switch To " + tweenName);
        Func<Tween> tweenFunc;
        if (!tweenDict.TryGetValue(tweenName, out tweenFunc))
        {
            Debug.LogWarning("Tween Not Exist");
            return;
        }
        SwitchToTween(tweenFunc);
    }

    public void RegisterTween(string name, Func<Tween> tween)
    {
        if (tweenDict.ContainsKey(name))
        {
            Debug.LogWarning("target Tween:" + name + " already registered");
            return;
        }
        tweenDict[name] = tween;
    }

    
}
