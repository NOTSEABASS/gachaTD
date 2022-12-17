using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkToCore : MonoBehaviour {
  void Start() {
    var seq = DOTween.Sequence();
    var init = transform.DOMove(new Vector3(0, 0.5f, 0) + Random.insideUnitSphere * 0.5f, 0.25f).SetRelative();
    var atk = transform.DOMove(PlayerCore.Instance.transform.position, 1).SetEase(Ease.InBack);
    seq.Append(init).Append(atk).Play().OnComplete(OnHit);
  }


  private void OnHit() {
    DealDamage();
    DOTween.Shake(
       () => CameraController.Instance.virtualOffset,
      (x) => CameraController.Instance.virtualOffset = x,
      0.17f, 0.1f * Vector3.one).Play();
    Destroy(gameObject);
  }

  private void DealDamage() {
    var data = PlayerDataManager.PlayerData;
    data.coreHealth--;
    PlayerDataManager.PlayerData = data;
  }
}
