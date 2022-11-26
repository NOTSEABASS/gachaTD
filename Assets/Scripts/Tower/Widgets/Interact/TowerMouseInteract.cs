using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerMouseInteract : MonoBehaviour, IOnLeftMouseDown, ClickFallback.IListener {


  public ClickFallback.Result OnClickFallback(MouseInputArgument arg) {
    if (arg.leftState == MouseInput.State.Down || arg.rightState == MouseInput.State.Down) {
      TowerInfoView.Instance.SetShow(false);
      return ClickFallback.Result.StopListen;
    }
    return ClickFallback.Result.None;
  }

  public MouseResult OnLeftMouseDown(MouseInputArgument arg) {
    TowerInfoView.Instance.SetShow(true);
    ClickFallback.Instance.AddListener(this);
    return MouseResult.BreakBehind;
  }
}
