using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PrepareSession : GameSession<PrepareSession> {

  public override void BeginSession() {
    Debug.Log("Enter Prepare Session");
    TowerManager.Instance.ResetAllTower();
  }

  public override void EndSession() {
    //Exit Prepare Session
  }

  public override bool CheckEnd() {
    return Input.GetKeyDown(KeyCode.Space);
  }

  public override GameSession Next() {
    return new BattleSession();
  }
}

public class BattleSession : GameSession<BattleSession> {
  public bool isEnd { set; private get; }

  public override void BeginSession() {
    Debug.Log("Enter Battle Seesion");
    EnemySpawnManager.Instance.StartSpawn();
  }

  public override void EndSession() {
  }

  public override bool CheckEnd() {
    return isEnd;
  }

  public override GameSession Next() {
    return new PrepareSession();
  }
}