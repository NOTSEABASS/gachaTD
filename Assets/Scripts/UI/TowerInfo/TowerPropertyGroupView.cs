using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class TowerPropertyGroupView : MonoBehaviour {
  [SerializeField]
  private TowerPropertyView propertyViewPrefab;
  [SerializeField]
  private Transform propertyContainer;

  private List<TowerPropertyView> propertyViews = new List<TowerPropertyView>();
  private int propertyPointer;

  public void Render(TowerData data) {
    foreach (var propertyView in propertyViews) {
      propertyView.Clear();
    }
    propertyPointer = 0;

    var property = GetPropertyViewByPointer();
    property.Render(TowerPropertyView.PropertyObject.AttackValue, data.atk);

    property = GetPropertyViewByPointer();
    property.Render(TowerPropertyView.PropertyObject.AttackRadius, data.atkRadius);

    property = GetPropertyViewByPointer();
    property.Render(TowerPropertyView.PropertyObject.AttackFreq, data.atkFreq);

    property = GetPropertyViewByPointer();
    property.Render(TowerPropertyView.PropertyObject.EnergyRecover, data.energyRecover);
  }

  private TowerPropertyView GetPropertyViewByPointer() {
    if (propertyPointer >= propertyViews.Count) {
      propertyViews.Add(Instantiate(propertyViewPrefab, propertyContainer));
    }
    return propertyViews[propertyPointer++];
  }
}
