using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPropertyGroupView : CharacterPropertyGroupView {
  public void Render(EnemyData data) {
    Clear();

    var property = GetPropertyViewByPointer();
    property.Render(CharacterPropertyView.PropertyObject.AttackValue, data.atk);

    property = GetPropertyViewByPointer();
    property.Render(CharacterPropertyView.PropertyObject.AttackRadius, data.atkRadius);

    property = GetPropertyViewByPointer();
    property.Render(CharacterPropertyView.PropertyObject.AttackFreq, data.atkFreq);
  }
}
