using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawnContext {
  public List<EnemySpawnVolume> volumes;
  public List<EnemySpawnInfo> infos;
  public bool isRelaxed;

  public void FillInfoBySpawnResources(EnemySpawnResources spawnResources) {
    infos = new List<EnemySpawnInfo>();
    var batches = spawnResources.batches;
    for (int batchIdx = 0; batchIdx < batches.Count; batchIdx++) {
      var entries = batches[batchIdx].entries;
      for (int entryIdx = 0; entryIdx < entries.Count; entryIdx++) {
        var entry = entries[entryIdx];
        for (int i = 0; i < entry.count; i++) {
          var info = new EnemySpawnInfo();
          info.name = entry.name;
          info.moveBatchIndex = batchIdx;
          infos.Add(info);
        }
      }
    }
  }
}

[Serializable]
public class EnemySpawnBatch {
  [Serializable]
  public struct Entry {
    public EnemyName name;
    public int count;
  }
  public List<Entry> entries = new List<Entry>();
}

public struct EnemySpawnVolume {
  public Vector2 pos;
  public float radius;

  public bool Overlap(EnemySpawnVolume other) {
    return Vector2.Distance(pos, other.pos) < radius + other.radius;
  }

  public Vector2 GetSeperatingVecotr(EnemySpawnVolume other) {
    var d = radius + other.radius;
    var curD = Vector2.Distance(pos, other.pos);
    return (pos - other.pos).normalized * (d - curD);
  }
}

public struct EnemySpawnInfo {
  public EnemyName name;
  public int moveBatchIndex;
}