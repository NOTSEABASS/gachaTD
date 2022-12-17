using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameSession {
  public abstract void BeginSession();
  public abstract void EndSession();

  public abstract bool CheckEnd();

  public abstract GameSession Next();

}

public abstract class GameSession<T> : GameSession where T : GameSession<T> {
  private static T instance;
  public static T Instnace => instance;
  public GameSession() {
    instance = this as T;
  }
}

public class GameFlowManager : MonoSingleton<GameFlowManager> {
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
