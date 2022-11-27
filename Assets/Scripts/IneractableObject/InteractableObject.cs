using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Outline))]
public abstract class InteractableObject : MonoBehaviour, IOnMousePureHover, IOnMouseExit {
    private Outline _outline;
    private void Awake() {
        _outline = GetComponent<Outline>();
    }

    public MouseResult OnMousePureHover(MouseInputArgument arg) {
        _outline.enabled = true;
        return MouseResult.None;
    }

    public MouseResult OnMouseExiting(MouseInputArgument arg) {
        _outline.enabled = false;
        return MouseResult.None;
    }
}
