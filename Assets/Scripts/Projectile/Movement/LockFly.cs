using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class LockFly : MonoBehaviour, IPoolCallback {
  public enum Curve {
    Direct, Parabolic
  }
  [SerializeField]
  private Curve curve;
  [SerializeField]
  private float speed;
  [SerializeField, ConditionalField(nameof(curve), inverse: false, Curve.Parabolic)]
  private float arcHeight;
  private Vector3? startPosition;
  public Transform target { set; private get; }

  private Vector3? lastValidPosition;

  public bool isArrived { get; private set; }

  public void OnGet() {
    isArrived = false;
    startPosition = null;
  }

  public bool HasValidTarget() {
    return target != null;
  }

  private void Update() {
    if (startPosition == null) {
      startPosition = transform.position;
    }

    bool isVliad = HasValidTarget();
    Vector3 targetPosition = Vector3.zero;
    if (isVliad) {
      lastValidPosition = target.position;
      transform.LookAt(target);
      targetPosition = target.position;
    } else {
      if (lastValidPosition != null) {
        targetPosition = (Vector3)lastValidPosition;
      } else {
        Debug.LogError("it really happen");
      }
    }

    var delta = targetPosition - transform.position;
    var step = speed * Time.deltaTime;
    if (delta.magnitude < step) {
      isArrived = true;
      transform.position = targetPosition;
    } else {
      switch (curve) {
        case Curve.Direct:
          transform.LookAt(targetPosition);
          transform.Translate(transform.forward * step, Space.World);
          break;
        case Curve.Parabolic:
          CurveUtils.ParabolicMove(transform, step, arcHeight, (Vector3)startPosition, targetPosition);
          break;
        default:
          break;
      }
    }
  }

}
