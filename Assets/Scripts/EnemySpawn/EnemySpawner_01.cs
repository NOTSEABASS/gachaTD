using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class EnemySpawner_01 : EnemySpawner {
  [SerializeField]
  private List<Transform> deployPoints;
  [SerializeField]
  private float spawnJoggle;

  private EnemySpawnContext spawnContext;

  //private void OnGUI() {
  //  if (WidgetGUILayout.Button("Spawn Test", nameof(EnemySpawner_01))) {
  //    Spawn_Test();
  //  }
  //}

  private void Update() {
    if (spawnContext == null) {
      return;
    }

    if (spawnContext.isRelaxed) {
      InstantiateContext(spawnContext);
      spawnContext = null;
    }
  }

  private void Spawn_Test() {
    spawnContext = new EnemySpawnContext();
    List<EnemySpawnVolume> volumes = new List<EnemySpawnVolume>();
    List<EnemySpawnInfo> infos = new List<EnemySpawnInfo>();
    foreach (var point in deployPoints) {
      var joggle = Random.insideUnitCircle * spawnJoggle;
      var volume = new EnemySpawnVolume();
      volume.pos = point.position.XZ() + joggle;
      volume.radius = 0.1f;
      volumes.Add(volume);

      var info = new EnemySpawnInfo();
      info.name = EnemyName.Wizard;
      infos.Add(info);
    }
    spawnContext.volumes = volumes;
    spawnContext.infos = infos;
    EnemyRelaxation.Instance.StartRelax(spawnContext);
  }

  private void InstantiateContext(EnemySpawnContext context) {
    for (int i = 0; i < context.volumes.Count; i++) {
      var volume = context.volumes[i];
      var info = context.infos[i];
      if (!ResourcesLoader.Instance.TryGetEnemyResources(info.name, out var resources)) {
        Debug.LogError("Invalid EnemyName: " + info.name);
        continue;
      }

      if (!PoolManager.Instance.GetObjectByPoolName<EnemyPoolObject>(resources.poolName, out var poolObject)) {
        Debug.LogError("Invalid PoolName: " + resources.poolName);
        continue;
      }

      poolObject.transform.position = new Vector3().SetXZ(volume.pos);
      var ptr = poolObject.FindDataPtr();
      if (EnemyDataHub.Instance.TryGetData(ptr, out var data)) {
        data.moveBatchIndex = info.moveBatchIndex;
        EnemyDataHub.Instance.SetData(ptr, data);
      } else {
        Debug.LogError("can't assign move batch index");
      }
    }
  }

  public override void Spawn(EnemySpawnContext context) {
    if (spawnContext != null) {
      return;
    }

    spawnContext = context;
    List<EnemySpawnVolume> volumes = new List<EnemySpawnVolume>();
    List<EnemySpawnInfo> infos = context.infos;

    for (int i = 0; i < infos.Count; i++) {
      var volume = new EnemySpawnVolume();
      volume.pos = GetRandomPosition();
      volume.radius = 0.25f;
      volumes.Add(volume);
    }

    spawnContext.volumes = volumes;
    spawnContext.infos = infos;
    EnemyRelaxation.Instance.StartRelax(spawnContext);
  }

  private Vector2 GetRandomPosition() {
    if (deployPoints.SafeCount() == 0) {
      return Vector3.zero;
    }

    var randomIdx = Random.Range(0, deployPoints.SafeCount());
    var randomPoint = deployPoints[randomIdx].position.XZ();
    var joggle = Random.insideUnitCircle * spawnJoggle;
    return randomPoint + joggle;
  }
}

