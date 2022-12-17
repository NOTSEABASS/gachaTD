using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GachaContext {
  public bool isUnboxed;
  public Transform gachaMount;
  public UnboxController unboxController;
  public GameObject gachaContent;

  private Collider collider;
  private DraggableObject draggableObject;

  public void SetGachaContent(GameObject content) {
    gachaContent = content;
    collider = content.GetComponent<Collider>();
    collider.enabled = false;
    draggableObject = content.GetComponent<DraggableObject>();
    draggableObject.SetInteractable(false);
  }

  public void OnUnbox() {
    isUnboxed = true;
    collider.enabled = true;
    draggableObject.SetInteractable(true);
  }

  public bool IsClearFromGachaMount() {
    return gachaMount.childCount == 0;
  }
}

public class GachaZone : MonoBehaviour {
  [SerializeField]
  private Transform[] tenGachaDropPoints;
  [SerializeField]
  private Transform singleGachaDropPoint;
  [SerializeField]
  private float dropHeight;
  [SerializeField]
  private float dropTime;

  private List<GachaContext> gachaContexts = new List<GachaContext>();

  public bool CheckGachable() {
    TryClearGachaZone();
    RemoveDeadContexts();
    var currentHasNoGacha = gachaContexts.SafeCount() == 0;
    return currentHasNoGacha;
  }

  public void PlaySingleGacha(UnboxController unbox) {
    ExecuteGacha(unbox, singleGachaDropPoint);
    GetDropGachaTween(unbox.transform, singleGachaDropPoint).Play();
  }

  public void PlayMultiyGacha(List<UnboxController> unboxes) {
    Debug.Assert(unboxes.Count <= tenGachaDropPoints.Length);
    TryClearGachaZone();
    var seq = DOTween.Sequence();
    var interval = 0.1f;
    var time = 0f;
    for (int i = 0; i < unboxes.Count; i++) {
      var unbox = unboxes[i];
      var mount = tenGachaDropPoints[i];
      ExecuteGacha(unbox, mount);
      var tween = GetDropGachaTween(unbox.transform, mount);
      seq.Insert(time, tween);
      time += interval;
    }
    seq.Play();
  }


  private void ExecuteGacha(UnboxController unbox, Transform mount) {
    var context = ConstructContext(unbox);
    context.gachaMount = mount;
    unbox.transform.parent = mount;
  }

  public void TryClearGachaZone() {
    RemoveDeadContexts();
    var seq = DOTween.Sequence();
    float time = 0f, interval = 0.1f;
    foreach (var context in gachaContexts) {
      if (!context.isUnboxed) {
        continue;
      }
      var emptySlot = ItemBoard_01.Instance.GetEmptySlot();
      if (emptySlot == null) {
        break;
      }

      context.gachaContent.transform.parent = emptySlot;
      var tween = GetMoveItemTween(context.gachaContent.transform);
      seq.Insert(time, tween);
      time += interval;
    }
    seq.Play();
  }

  private Tween GetMoveItemTween(Transform content) {
    return content.DOLocalMove(Vector3.zero, 1f).SetEase(Ease.OutQuart);
  }

  private void RemoveDeadContexts() {
    gachaContexts.RemoveAll(x => x.IsClearFromGachaMount());
  }

  private GachaContext ConstructContext(UnboxController unbox) {
    var context = new GachaContext();
    gachaContexts.Add(context);
    unbox.FillContext(context);
    return context;
  }

  private Tween GetDropGachaTween(Transform unbox, Transform mount) {
    unbox.position = mount.position + dropHeight * Vector3.up;
    var tween = unbox.DOMove(mount.transform.position, dropTime).SetEase(Ease.OutQuart);
    return tween;
  }
}
