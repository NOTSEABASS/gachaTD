using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public static class TransformEffects {
  public static Tween Shake(this Transform transform, float time) {
    Sequence sequence = DOTween.Sequence();
    var min = Vector3.one;
    var max = Vector3.one * 1.2f;
    sequence.Append(transform.DOScale(max, time / 2).SetEase(Ease.OutQuart));
    sequence.Append(transform.DOScale(min, time / 2).SetEase(Ease.OutQuart));

    return sequence;
  }

  public static Tween ShakeRotate(this Transform transform, Quaternion rotation, float time) {
    return transform.DORotateQuaternion(rotation, time).SetEase(Ease.OutQuart);
  }

  public static Tween DownToFlow(this Transform transform, float time) {
    Sequence sequence = DOTween.Sequence();
    var origin = transform.position.y;
    var targetPos = origin + 0.1f;
    sequence.Append(transform.DOMoveY(targetPos, time / 2).SetEase(Ease.OutQuart));
    return sequence;
  }

  public static Tween FlowToDown(this Transform transform, float time) {
    Sequence sequence = DOTween.Sequence();
    var origin = transform.position.y;
    var targetPos = origin - 0.1f;
    sequence.Append(transform.DOMoveY(targetPos, time / 2).SetEase(Ease.OutQuart));
    return sequence;
  }

  public static Tween FlowAndTilt(this Transform transform, float time) {
    Sequence sequence = DOTween.Sequence();
    var origin = transform.position.y;
    var targetPos = origin + 0.1f;
    var newDir = Vector3.forward + Vector3.one * 0.3f;

    sequence.Append(transform.DOMoveY(targetPos, time).SetEase(Ease.OutQuart));
    sequence.Join(transform.DORotateQuaternion(Quaternion.LookRotation(newDir, Vector3.up), time).SetEase(Ease.OutQuart));
    return sequence;
  }

  public static Tween DownAndTilt(this Transform transform, float time) {
    Sequence sequence = DOTween.Sequence();
    var origin = transform.position.y;
    var targetPos = origin - 0.1f;
    var newDir = Vector3.forward;

    sequence.Append(transform.DOMoveY(targetPos, time).SetEase(Ease.OutQuart));
    sequence.Join(transform.DORotateQuaternion(Quaternion.LookRotation(newDir, Vector3.up), time).SetEase(Ease.OutQuart));
    return sequence;
  }
}
