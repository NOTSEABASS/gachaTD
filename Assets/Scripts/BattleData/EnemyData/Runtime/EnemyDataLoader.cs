using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyDataLoader : MonoSingleton<EnemyDataLoader> {
  [SerializeField]
  protected EnemyTemplateSet templateSet;

  //todo: return dynamic data , instead of static serialized data
  public virtual EnemyData LoadData(EnemyTemplateId template) {
    if (templateSet == null) {
      return default;
    }

    if (!templateSet.ContainsKey(template)) {
      Debug.LogError(template);
    }
    return templateSet[template].GetData();
  }

}
