using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Net.Http.Headers;
using MyBox;

public class UnboxController : MonoBehaviour {
  [SerializeField]
  private Transform[] innerTopAxis;
  [SerializeField]
  private Transform[] outerTopAxis;
  [SerializeField]
  private float innerMax;
  [SerializeField]
  private float outerMax;
  [SerializeField]
  private Transform slider;
  [SerializeField]
  private Transform sliderOrigin;
  [SerializeField]
  private Transform sliderEnd;
  [SerializeField]
  private bool usePreview;
  [SerializeField, Range(0f, 1f)]
  private float preview;
  [SerializeField]
  private MeshRenderer[] renderers;
  [SerializeField, Range(0f, 1f)]
  private float alpha;
  [SerializeField]
  private Transform contentRoot;
  [SerializeField, AutoProperty]
  private Collider interactCollider;
  private float innerAngle;
  private float outerAngle;

  private float slideNormalized;

  private bool isPlayingUnbox;

  private GameObject gachaContent;
  private GachaContext gachaContext;
  private Tween unboxTween;

  private void Update() {
    if (usePreview) {
      if (unboxTween == null) {
        unboxTween = ConstructTween().SetUpdate(UpdateType.Manual);
      }
      unboxTween.Goto(preview);
    }
  }

  public Vector3 GetSliderVectorInCamera() {
    var a = sliderOrigin.position;
    a = Camera.main.WorldToScreenPoint(a);
    var b = sliderEnd.position;
    b = Camera.main.WorldToScreenPoint(b);
    return b - a;
  }

  public void FillContext(GachaContext context) {
    context.unboxController = this;
    gachaContext = context;
    context.SetGachaContent(gachaContent);
  }

  public void SetContent(GameObject content) {
    this.gachaContent = content;
    content.transform.parent = contentRoot;
    content.transform.localPosition = Vector3.zero;
    contentRoot.transform.localScale = 0.5f * Vector3.one;
  }

  public void MoveSlider(float deltaNormalized) {
    slideNormalized = Mathf.Clamp01(slideNormalized + deltaNormalized);
    slider.transform.position = Vector3.Lerp(sliderOrigin.position, sliderEnd.position, slideNormalized);
    if (slideNormalized > 0.99f) {
      PlayUnbox();
    }
  }

  public void PlayUnbox() {
    if (isPlayingUnbox) return;

    isPlayingUnbox = true;
    interactCollider.enabled = false;
    slider.gameObject.SetActive(false);
    ConstructTween().OnComplete(OnUnboxComplete).Play();
  }

  private void OnUnboxComplete() {
    if (gachaContext != null) {
      gachaContext.OnUnbox();
    }
    Destroy(gameObject);
  }

  private void UnparentContenr() {
    if (gachaContent != null) {
      gachaContent.transform.parent = transform.parent;
    }
  }

  private Tween ConstructTween() {
    Sequence seq = DOTween.Sequence();
    var inner = DOTween.To(() => innerAngle, SetInnerAxis, innerMax, 0.8f).SetEase(Ease.OutBack, overshoot: 1.3f);
    var outer = DOTween.To(() => outerAngle, SetOuterAxis, outerMax, 0.8f).SetEase(Ease.OutBack, overshoot: 1.3f);
    var swell = contentRoot.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack).OnComplete(UnparentContenr);
    var collapse = transform.DOScaleY(0.1f, 0.25f);
    var fadeout = transform.DOMoveY(-0.5f, 0.15f).SetRelative();
    seq.Append(outer).Insert(0.1f, inner).Insert(0.5f, collapse).Insert(0.1f, swell).Append(fadeout).Play();
    return seq;
  }

  private void SetInnerAxis(float x) {
    var delta = x - innerAngle;
    innerAngle = x;
    foreach (var axis in innerTopAxis) {
      axis.Rotate(Vector3.right, delta, Space.Self);
    }
  }

  private void SetOuterAxis(float x) {
    var delta = x - outerAngle;
    outerAngle = x;
    foreach (var axis in outerTopAxis) {
      axis.Rotate(Vector3.right, delta, Space.Self);
    }
  }

}
