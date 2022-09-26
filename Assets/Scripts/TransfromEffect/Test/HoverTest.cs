using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverTest : MonoBehaviour {
  public int testIndex;
  private SwitchTween switchTween;

  private void Awake() {
    switchTween = new TestCubeTween(transform);
  }

  public void OnMouseEnter() {
    switch (testIndex) {
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
    }
  }

  public void OnMouseExit() {
    switch (testIndex) {
      case 2:
        switchTween.SwitchToTween("down");
        break;
      case 3:
        switchTween.SwitchToTween("downTilt");
        break;
    }
  }
}
