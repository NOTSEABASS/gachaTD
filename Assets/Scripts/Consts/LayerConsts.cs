using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LayerConsts {
  public static LayerMask EnemyMask => LayerMask.GetMask("Enemy");
  public static int EnemyLayer => LayerMask.NameToLayer("Enemy");
  public static int DraggingMountLayer => LayerMask.NameToLayer("DraggingMount");

}
