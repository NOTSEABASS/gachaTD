using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PapaWorm : EnemyBase {
  public void OnDataChange(EnemyData data) {
    print(data.hp);
  }
}
