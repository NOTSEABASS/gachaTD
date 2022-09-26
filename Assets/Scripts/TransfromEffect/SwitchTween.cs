using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core.Enums;
using UnityEngine;


public class UniqueTween {
  private Tween m_tween;
  public Tween Tween => m_tween;

  public void SetAndPlay(Tween tween, bool finishLastOne = true) {
    if (m_tween.IsActive()) {
      if (finishLastOne) {
        m_tween.Goto(m_tween.Duration());
      }
      m_tween.Kill();
    }
    m_tween = tween.Play();
  }

  public void SetAfterKillAndPlay(Func<Tween> tweenFunc, bool finishLastOne = true) {
    if (m_tween.IsActive()) {
      if (finishLastOne) {
        m_tween.Goto(m_tween.Duration());
      }
      m_tween.Kill();
    }
    m_tween = tweenFunc.Invoke().Play();
  }

}

public abstract class SwitchTween {
  private UniqueTween uniqueTween = new UniqueTween();
  private Dictionary<string, Func<Tween>> tweenDict;

  public SwitchTween() {
    tweenDict = new Dictionary<string, Func<Tween>>();
    OnInit();
  }

  protected abstract void OnInit();

  private void SwitchToTween(Func<Tween> tween, bool finishLastOne) {
    uniqueTween.SetAfterKillAndPlay(tween, finishLastOne);
  }

  public void SwitchToTween(string tweenName, bool finishLastOne = true) {
    Func<Tween> tweenFunc;
    if (!tweenDict.TryGetValue(tweenName, out tweenFunc)) {
      Debug.LogError("Tween Not Exist");
      return;
    }
    SwitchToTween(tweenFunc, finishLastOne);
  }

  protected void RegisterTween(string name, Func<Tween> tween) {
    if (tweenDict.ContainsKey(name)) {
      Debug.LogError("target Tween:" + name + " already registered");
      return;
    }
    tweenDict[name] = tween;
  }


}
