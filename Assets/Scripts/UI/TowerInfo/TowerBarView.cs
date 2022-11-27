using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBarView : MonoBehaviour {
  [SerializeField]
  private BarView hpBarView;
  [SerializeField]
  private BarView energyBarView;

  public void Render(TowerData data) {
    var model = new BarView.Model();
    model.current = data.hp;
    model.max = data.maxHp;
    hpBarView.Render(model);

    model.current = data.energy;
    model.max = data.maxEnergy;
    energyBarView.Render(model);
  }
}
