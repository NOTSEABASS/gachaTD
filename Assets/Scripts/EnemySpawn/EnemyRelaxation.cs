using System.Collections;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using UnityEngine;
using System;
using UnityEngine.Rendering;

public class EnemyRelaxation : MonoSingleton<EnemyRelaxation> {
  [SerializeField]
  private int interateTime;

  private EnemySpawnContext spawnContext;

  public void StartRelax(EnemySpawnContext spawnContext) {
    this.spawnContext = spawnContext;
  }

  public void Update() {
    if (spawnContext != null && !spawnContext.isRelaxed) {
      for (int i = 0; i < interateTime; i++) {
        Iterate();
      }
      spawnContext.isRelaxed = true;
      spawnContext = null;
    }
  }

  private void OnGUI() {
    if (WidgetGUILayout.Button("Relax Iterate", "Relax Iterate")) {
      for (int i = 0; i < interateTime; i++) {
        Iterate();
      }
    }
  }

  private void Iterate() {
    var count = spawnContext.volumes.Count;
    var volumes = spawnContext.volumes;

    for (int i = 0; i < count; i++) {
      var volume = volumes[i];
      var push = Vector2.zero;
      for (int j = 0; j < count; j++) {
        if (j == i) {
          continue;
        }

        var otherVolume = volumes[j];
        if (volume.Overlap(otherVolume)) {
          push += volume.GetSeperatingVecotr(otherVolume);
        }
      }
      volume.pos += push;
      volumes[i] = volume;
    }

  }

  private void OnDrawGizmos() {
    if (spawnContext == null) {
      return;
    }

    var volumes = spawnContext.volumes;
    if (volumes.SafeCount() > 0) {
      for (int i = 0; i < volumes.Count; i++) {
        var volume = volumes[i];
        Gizmos.DrawWireSphere(new Vector3().SetXZ(volume.pos), volume.radius);
      }
    }
  }

}
