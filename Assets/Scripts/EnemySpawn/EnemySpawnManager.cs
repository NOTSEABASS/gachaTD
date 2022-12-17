using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class EnemySpawner : MonoBehaviour {
  public abstract void Spawn(EnemySpawnContext context);
}

public class EnemySpawnManager : MonoSingleton<EnemySpawnManager> {
  [SerializeField]
  private EnemySpawner spawner;
  [SerializeField]
  private EnemySpawnResources spawnResources;

  public void StartSpawn() {
    var context = new EnemySpawnContext();
    context.FillInfoBySpawnResources(spawnResources);
    spawner.Spawn(context);

    StartCoroutine(DelayRelease());
  }

  private void OnGUI() {
    //if (WidgetGUILayout.Button("Start Spawn")) {
    //  StartSpawn();
    //  StartCoroutine(DelayRelease());
    //}
  }

  private IEnumerator DelayRelease() {
    yield return new WaitForSeconds(1);
    EnemyMoveBatchManager.Instance.ResetState();
    EnemyMoveBatchManager.Instance.StartRelease();
  }
}
