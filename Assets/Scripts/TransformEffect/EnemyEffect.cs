using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEffect : MonoBehaviour, IPoolCallback {
  private Collider[] colliders;
  private bool isDead;

  private void Awake() {
    colliders = GetComponentsInChildren<Collider>();
  }
  public void OnDeath() {
    //isDead = true;
    //foreach (var collider in colliders) {
    //  collider.enabled = false;
    //}
    transform.DOShakePosition(0.25f, 0.75f).Play();
    transform.DOScale(0F, 0.25f).Play();
  }

  void IPoolCallback.OnGet() {
    //if (isDead) { //判断IsDead是避免在第一次从池中取出来的时候执行碰撞盒开启
    //  isDead = false;
    //  foreach (var collider in colliders) {
    //    collider.enabled = true;
    //  }
    //}
    transform.localPosition = Vector3.zero;
    transform.localScale = Vector3.one;
  }

}
