using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPropertyGroupView : MonoBehaviour
{
  [SerializeField]
  private CharacterPropertyView propertyViewPrefab;
  [SerializeField]
  private Transform propertyContainer;

  private List<CharacterPropertyView> propertyViews = new List<CharacterPropertyView>();
  private int propertyPointer;


  protected CharacterPropertyView GetPropertyViewByPointer() {
    if (propertyPointer >= propertyViews.Count) {
      propertyViews.Add(Instantiate(propertyViewPrefab, propertyContainer));
    }
    return propertyViews[propertyPointer++];
  }

  protected void Clear() {
    foreach (var propertyView in propertyViews) {
      propertyView.Clear();
    }
    propertyPointer = 0;
  }

}
