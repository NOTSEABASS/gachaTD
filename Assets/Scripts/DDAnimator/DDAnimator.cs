using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DDAnimator : MonoBehaviour {
  public enum UpdateMode {
    Manual,
    Loop
  }

  [System.Serializable]
  private struct Status {
    public UpdateMode mode;
    [ConditionalField("mode", false, UpdateMode.Loop)]
    public float loopTime;
    [ConditionalField("mode", false, UpdateMode.Loop)]
    public float loopTimer;
    public string stateName;
    public float stateOffset;
    //过渡相关
    public bool isTransiting;
    public string nextStateName;
    public float transitionOrigin;
    public float nextStateOffset;
    public float normalizedTransitionDuration;
    public float lastApplyNormalizedTime;


  }

  [SerializeField, AutoProperty]
  private Animator animator;
  [SerializeField, ReadOnly]
  private Status status;

  private void Awake() {
    animator.speed = 0;
    SetUpdateMode(UpdateMode.Manual);
  }

  public void Update() {
    if (status.mode == UpdateMode.Loop) {
      Play(status.loopTimer / status.loopTime);
      status.loopTimer += Time.deltaTime;
      status.loopTimer %= status.loopTime;
    }
  }

  public void SetUpdateMode(UpdateMode mode, float loopTime = 0) {
    if (mode == UpdateMode.Loop && loopTime <= 0) {
      Debug.LogError("[DDAnimator] Illegal loopTime for Loop mode");
      loopTime = 0.1f;
    }

    status.mode = mode;
    status.loopTime = loopTime;
    if (status.mode == UpdateMode.Loop) {
      status.loopTimer = status.loopTime * status.lastApplyNormalizedTime;
    }
  }


  public bool SetState(string stateName, float stateOffset, float transitDuration = 0.15f) {
    var statusIsEmpty = status.stateName == "";
    var equalToState = stateName == status.stateName && stateOffset == status.stateOffset;
    var equalToNextState = stateName == status.nextStateName && stateOffset == status.nextStateOffset;

    if ((!status.isTransiting && equalToState) ||
         (status.isTransiting && equalToNextState)) {
      return false;
    }

    if (transitDuration <= 0 || statusIsEmpty) {
      status.isTransiting = false;
      status.stateName = stateName;
      status.stateOffset = stateOffset;
    } else {
      if (status.isTransiting) {
        //apply last transition before start new transition
        status.stateName = status.nextStateName;
        status.stateOffset = status.nextStateOffset;
        animator.Play(status.stateName, 0, status.lastApplyNormalizedTime);
        animator.Update(0);
        /*TO-DO：如果在过渡过程中，需要开始新的过渡，那么需要链式记录老的过渡，再计算新的过渡时用遍历这条链
         *才能获得完全准确的结果。
         *目前的版本在过渡中开始新过渡有可能会看到闪动
         */
      }

      status.isTransiting = true;
      status.transitionOrigin = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
      status.nextStateName = stateName;
      status.nextStateOffset = stateOffset;
      status.normalizedTransitionDuration = transitDuration;
      status.lastApplyNormalizedTime = 0;
      status.loopTimer = 0;
    }

    return true;
  }

  public void Play(float normalizedTime) {
    normalizedTime = Mathf.Clamp01(normalizedTime);
    if (status.isTransiting) {
      {
        //still in transition
        if (normalizedTime <= status.normalizedTransitionDuration) {
          animator.Play(status.stateName, 0, status.transitionOrigin);
          animator.Update(0);

          var transitTime = normalizedTime / status.normalizedTransitionDuration; //will not devide zero
          animator.CrossFade(stateName: status.nextStateName,
                             layer: 0,
                             normalizedTransitionDuration: status.normalizedTransitionDuration,
                             normalizedTimeOffset: status.nextStateOffset + normalizedTime,
                             normalizedTransitionTime: transitTime);
          //quit transition
        } else {
          status.isTransiting = false;
          status.stateName = status.nextStateName;
          status.stateOffset = status.nextStateOffset;
        }
      }
    }
    if (!status.isTransiting) {
      animator.Play(stateName: status.stateName,
                    layer: 0,
                    normalizedTime: normalizedTime + status.stateOffset);
      animator.Update(0);
    }
    status.lastApplyNormalizedTime = normalizedTime;
  }
}
