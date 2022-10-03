using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHoverActivator : MonoBehaviour, IOnMouseHover {

  [SerializeField]
  private MonoBehaviour[] components;
  [SerializeField]
  private GameObject[] gameObjects;
  private bool activeOnCurrentFrame;
  public MouseResult OnMouseHover(MouseInputArgument arg) {
    SetTargetsActive(true);
    activeOnCurrentFrame = true;
    return MouseResult.None;
  }

  public void Update() {
    if (!activeOnCurrentFrame) {
      SetTargetsActive(false);
    }
    activeOnCurrentFrame = false;
  }

  private void SetTargetsActive(bool isActive) {
    for (int i = 0, count = components.SafeCount(); i < count; i++) {
      components[i].enabled = isActive;
    }
    for (int i = 0, count = gameObjects.SafeCount(); i < count; i++) {
      gameObjects[i].SetActive(isActive);
    }
  }
}
