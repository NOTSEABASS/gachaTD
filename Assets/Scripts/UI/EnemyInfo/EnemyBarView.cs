using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBarView : MonoBehaviour
{
  [SerializeField]
  private BarView hpBarView;

  public void Render(EnemyData data) {
    var model = new BarView.Model();
    model.current = data.hp;
    model.max = data.maxHp;
    hpBarView.Render(model);
  }
}
