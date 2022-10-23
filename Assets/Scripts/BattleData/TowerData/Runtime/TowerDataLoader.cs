using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDataLoader : MonoSingleton<TowerDataLoader>
{
  [SerializeField]
  protected TowerTemplateSet templateSet;

  //todo: return dynamic data , instead of static serialized data
  public virtual TowerData LoadData(TowerDataTemplateId template) {
    if (templateSet == null) {
      return default;
    }

    if (!templateSet.ContainsKey(template)) {
      Debug.LogError(template);
    }
    return templateSet[template].GetData();
  }
}
