using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCoreDamage : MonoSingleton<EnemyCoreDamage> {
  [SerializeField]
  private AtkToCore atkPrefab;

  public void DealDamage(Vector3 position, int enemyPtr) {
    var atk = Instantiate(atkPrefab);
    atk.transform.position = position;
  }

}
