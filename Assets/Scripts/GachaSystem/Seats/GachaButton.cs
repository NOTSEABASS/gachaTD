using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class GachaButton : InteractableObject, IOnLeftMouseDown {
  [SerializeField] private UnityEvent OnButtonPressed;
  private UniqueTween moveUniqueTween;

  protected override void Awake() {
    base.Awake();
    moveUniqueTween = new UniqueTween();
  }

  public MouseResult OnLeftMouseDown(MouseInputArgument arg) {
    ButtonPressTween();
    OnButtonPressed.Invoke();
    return MouseResult.None;
  }

  private void ButtonPressTween() {
    var t = transform.GetChild(0);
    var tween = t.DoPressed(0.05f, 0.1f);
    moveUniqueTween.SetAndPlay(tween);
  }

  public MouseResult OnLeftMouseUp(MouseInputArgument arg) {
    return MouseResult.None;
  }
}
