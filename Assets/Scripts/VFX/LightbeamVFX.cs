using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using DG.Tweening;

public class LightbeamVFX : PoolObject {
  [SerializeField]
  private Transform scale;
  [SerializeField]
  private Vector2 range;

  private Vector3 from;
  private Vector3 to;
  public void Render(Vector3 from, Vector3 to) {
    this.to = to;
    this.from = from;
    transform.SetLocalScaleX(range.x);
    transform.SetLocalScaleY(range.x);
    transform.DOScaleX(range.y, 0.17f).Play();
    transform.DOScaleY(range.y, 0.17f).Play();
    transform.position = from;
    DOTween.To(() => from, SetFromPosition, to, 0.17f).Play();
    transform.LookAt(to);
    scale.SetLocalScaleZ(Vector3.Distance(from, to));
  }

  private void SetFromPosition(Vector3 position) {
    from = position;
    transform.position = position;
    scale.SetLocalScaleZ(Vector3.Distance(from, to));
  }
}
