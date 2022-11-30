using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMouseInteract : MonoBehaviour, IOnLeftMouseUp, IOnMouseHover {
  [SerializeField]
  private Outline outline;

  private bool hover;

  private void Update() {
    if (hover) {
      hover = false;
    } else if (outline.enabled) {
      outline.enabled = false;
    }
  }
  public MouseResult OnLeftMouseUp(MouseInputArgument arg) {
    var ptr = this.FindDataPtr();
    if (EnemyDataHub.Instance.TryGetData(ptr, out var data)) {
      InfoPanelView.Instance.SetShow(true);
      InfoPanelView.Instance.ShowViewObject(InfoPanelView.Object.EnemyInfo);
      print(EnemyInfoView.Instance);
      EnemyInfoView.Instance.Render(data);
    }
    return MouseResult.BreakBehind;
  }

  public MouseResult OnMouseHover(MouseInputArgument arg) {
    hover = true;
    outline.enabled = true;
    return MouseResult.None;
  }


}
