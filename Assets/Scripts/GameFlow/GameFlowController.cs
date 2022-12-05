using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Sessions
public abstract class GameSession {
  public abstract void BeginSession();
  public abstract void EndSession();

  public abstract bool CheckEnd();

  public abstract GameSession Next();

}

public class PrepareSession : GameSession {
  public override void BeginSession() {
    //Enter Prepare Session
    Debug.Log("Prepare");
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

public class BattleSession : GameSession {
  public override void BeginSession() {
    //Enter Battle Session
    Debug.Log("Battle");
  }

  public override void EndSession() {
    //Exit Battle Session
  }

  public override bool CheckEnd() {
    return Input.GetKeyDown(KeyCode.Space);
  }

  public override GameSession Next() {
    return new PrepareSession();
  }
}
#endregion


public class GameFlowController : MonoSingleton<GameFlowController>
{
  private GameSession currentSession;
  public String CurrentSession => currentSession.ToString();
  private void Start() {
    currentSession = new PrepareSession();
    currentSession.BeginSession();
  }

  private void GotoNextSession() {
    currentSession.EndSession();
    currentSession = currentSession.Next();
    currentSession.BeginSession();

  }

  private void Update() {
    if (currentSession.CheckEnd()) {
      GotoNextSession();
    }
  }
}
