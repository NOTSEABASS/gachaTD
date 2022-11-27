using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class GachaButton : InteractableObject, IOnLeftMouseDown {
  [SerializeField] private UnityEvent OnButtonPressed;
  private float originY;
  private void Start() {
    originY = transform.position.y;
  }

  private void Update() {
    
  }

  public MouseResult OnLeftMouseDown(MouseInputArgument arg) {
    OnButtonPressed.Invoke();
    return MouseResult.None;
  }

  public void ButtonPressTween() {
    var t = transform.GetChild(0);
    UniqueTween moveUniqueTween = new UniqueTween();
    var tween = t.Pressed(0.05f, 0.3f);
    moveUniqueTween.SetAndPlay(tween);
  }
}
