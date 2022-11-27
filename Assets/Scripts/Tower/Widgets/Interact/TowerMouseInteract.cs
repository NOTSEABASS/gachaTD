using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerMouseInteract : MonoBehaviour, IOnLeftMouseUp, ClickFallback.IListener {
  public ClickFallback.Result OnClickFallback(MouseInputArgument arg) {
    if (arg.leftState == MouseInput.State.Up || arg.rightState == MouseInput.State.Up) {
      TowerInfoView.Instance.SetShow(false);
      return ClickFallback.Result.StopListen;
    }
    return ClickFallback.Result.None;
  }

  public MouseResult OnLeftMouseUp(MouseInputArgument arg) {
    var ptr = this.FindDataPtr();
    if (TowerDataHub.Instance.TryGetData(ptr, out var data)) {
      TowerInfoView.Instance.SetShow(true);
      TowerInfoView.Instance.Render(data);
      ClickFallback.Instance.AddListener(this);
    }
    return MouseResult.BreakBehind;
  }
}
