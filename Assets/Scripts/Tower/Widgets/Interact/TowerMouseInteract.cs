using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerMouseInteract : MonoBehaviour, IOnLeftMouseUp{
  public MouseResult OnLeftMouseUp(MouseInputArgument arg) {
    var ptr = this.FindDataPtr();
    if (TowerDataHub.Instance.TryGetData(ptr, out var data)) {
      InfoPanelView.Instance.SetShow(true);
      InfoPanelView.Instance.ShowViewObject(InfoPanelView.Object.TowerInfo);
      TowerInfoView.Instance.Render(data);
    }
    return MouseResult.BreakBehind;
  }
}
