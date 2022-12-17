using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveBatchManager : MonoSingleton<EnemyMoveBatchManager> {
  private Dictionary<int, List<int>> batchToPtrs = new Dictionary<int, List<int>>();
  private HashSet<int> currentAlives = new HashSet<int>();
  private int currentMoveBatch = -1;
  private bool isReleasing;

  private void Update() {
    if (!isReleasing) {
      return;
    }

    if (currentMoveBatch < 0 || CheckCurrentBatchClear()) {
      TryReleaseBatch(++currentMoveBatch);
    }
  }

  private bool CheckCurrentBatchClear() {
    return currentAlives.Count == 0;
  }

  public void StartRelease() {
    isReleasing = true;
  }

  public void ResetState() {
    currentMoveBatch = -1;
  }

  private void OnAllBatchClear() {
    isReleasing = false;
    BattleSession.Instnace.isEnd = true;
  }

  private void TryReleaseBatch(int batchIdx) {
    if (!batchToPtrs.ContainsKey(batchIdx)) {
      OnAllBatchClear();
      return;
    }
    var list = batchToPtrs[batchIdx];
    foreach (var ptr in list) {
      if (!EnemyDataHub.Instance.TryGetData(ptr, out var data)) {
        continue;
      }

      data.isInMoveBatch = true;
      data.UpdateVersion();
      EnemyDataHub.Instance.SetData(ptr, data);
      currentAlives.Add(ptr);
    }
    list.Clear();
  }

  public void OnEnemyDeath(int ptr) {
    currentAlives.Remove(ptr);
  }

  public void Register(int batchIdx, int ptr) {
    var list = GetPtrList(batchIdx);
    list.Add(ptr);
  }

  private List<int> GetPtrList(int batchIdx) {
    if (!batchToPtrs.ContainsKey(batchIdx)) {
      batchToPtrs[batchIdx] = new List<int>();
    }
    return batchToPtrs[batchIdx];
  }


}
