using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public static class TransformEffects 
{
    public static void Shake(this Transform transform, float time)
    {
        Sequence shakeSequence = DOTween.Sequence();
        var min = transform.localScale;
        var max = transform.localScale * 1.2f;
        shakeSequence.Append(transform.DOScale(max, time / 2).SetEase(Ease.OutQuart));
        shakeSequence.Append(transform.DOScale(min, time / 2).SetEase(Ease.OutQuart));
    }
    
    public static void ShakeRotate(this Transform transform, Quaternion rotation, float time)
    {
        transform.DORotateQuaternion(rotation, time).SetEase(Ease.OutQuart);
    }
}
