using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class TowerPropertyGroupView : CharacterPropertyGroupView {
  public void Render(TowerData data) {
    Clear();

    var property = GetPropertyViewByPointer();
    property.Render(CharacterPropertyView.PropertyObject.AttackValue, data.atk);

    property = GetPropertyViewByPointer();
    property.Render(CharacterPropertyView.PropertyObject.AttackRadius, data.atkRadius);

    property = GetPropertyViewByPointer();
    property.Render(CharacterPropertyView.PropertyObject.AttackFreq, data.atkFreq);

    property = GetPropertyViewByPointer();
    property.Render(CharacterPropertyView.PropertyObject.EnergyRecover, data.energyRecover);
  }
}
