using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour {
  [SerializeField]
  private Camera targetCamera;
  [SerializeField]
  private Vector2 zoomRange;
  [SerializeField]
  private int zoomLevel;
  [SerializeField]
  private float zoomUnitLength;
  [SerializeField]
  private float zoomDuration;
  [SerializeField]
  private float rotateDuration;
  [SerializeField]
  private int rotateLevel;
  [SerializeField]
  private float moveSpeed;
  [SerializeField]
  private float moveDuration;
  [SerializeField]
  private float focusHeight;
  private Transform targetTransform => targetCamera.transform;

  private Vector3 originVirtualPosition;
  private Vector3 originFlattenForward;


  private const int MAX_ROTATE_LEVEL = 4; // hard-coded 4 direction

  private Vector3 virtualPositoin; // x-y for plane move, z for zoom
  private float yawAxisAngle;

  private UniqueTween zoomUniqueTween = new UniqueTween();
  private UniqueTween rotateUniqueTween = new UniqueTween();
  private UniqueTween moveUniqueTween = new UniqueTween();

  private void Start() {
    originFlattenForward = targetTransform.forward;
    originFlattenForward.y = 0;
    originVirtualPosition = GetOriginVirtualPosition();
  }

  void Update() {
    var scrollInput = Input.GetAxis("Mouse ScrollWheel");
    if (scrollInput > 0) {
      ChangeZoomLevel(+1);
      StartZoomTween();
      return;
    }
    if (scrollInput < 0) {
      ChangeZoomLevel(-1);
      StartZoomTween();
      return;
    }


    if (Input.GetKeyDown(KeyCode.Q)) {
      rotateLevel = (rotateLevel + 1); //没有求余，因为会导致360-0的边界错误，所以没做
      StartRotateTween();
      return;
    }
    if (Input.GetKeyDown(KeyCode.E)) {
      rotateLevel = (rotateLevel - 1);
      StartRotateTween();
      return;
    }

    var horizontalInput = Input.GetAxis("Horizontal");
    var verticalInput = Input.GetAxis("Vertical");
    var deltaPosition = new Vector2(horizontalInput, verticalInput);
    if (deltaPosition != Vector2.zero) {
      StartMoveTween(deltaPosition.normalized * moveSpeed);
    }
  }

  private void StartMoveTween(Vector2 deltaPlanePosition) {
    moveUniqueTween.SetAndPlay(GetMoveTween(deltaPlanePosition), finishLastOne: false);
  }

  private void StartZoomTween() {
    zoomUniqueTween.SetAndPlay(GetZoomTween(), finishLastOne: false);
  }

  private void StartRotateTween() {
    rotateUniqueTween.SetAndPlay(GetRotateTween(), finishLastOne: false);
  }

  private Vector3 GetOriginVirtualPosition() {
    var mat = targetTransform.worldToLocalMatrix;
    var worldFixedPointInLocal = mat.MultiplyPoint(Vector3.zero);
    return worldFixedPointInLocal;
  }

  private void SetPlanePosition(Vector2 pos) {
    virtualPositoin = virtualPositoin.SetXY(pos);
    RecaculateTransform();
  }

  private void SetAxisDistance(float distance) {
    virtualPositoin = virtualPositoin.SetZ(distance);
    RecaculateTransform();
  }

  private void SetYawAxisAngle(float deg) {
    yawAxisAngle = deg;
    RecaculateTransform();
  }
  private void RecaculateTransform() {
    var mat = targetTransform.worldToLocalMatrix;

    //virtual position 
    var worldFixedPointInLocal = mat.MultiplyPoint(Vector3.zero);
    var newLocalPosition = worldFixedPointInLocal + virtualPositoin - originVirtualPosition;

    targetTransform.position = targetTransform.localToWorldMatrix.MultiplyPoint(newLocalPosition);

    //yaw axis angle
    var focusPoint = GetFocusPoint();
    var flattenForward = targetTransform.forward;
    flattenForward.y = 0;

    var targetFlatten = Quaternion.AngleAxis(yawAxisAngle, Vector3.up) * originFlattenForward;
    var delta = Vector3.SignedAngle(flattenForward, targetFlatten, Vector3.up);

    targetTransform.RotateAround(focusPoint, Vector3.up, delta);
  }

  //move 和 zoom不应该被合并， 因为这是两个可以并行Tween的操作
  private Tween GetMoveTween(Vector2 delta) {
    var targetPosition = virtualPositoin.XY() + delta;
    return DOTween.To(() => virtualPositoin.XY(), SetPlanePosition, targetPosition, moveDuration).SetEase(Ease.OutQuint);
  }

  private Tween GetZoomTween() {
    var targetDistance = zoomUnitLength * zoomLevel;
    return DOTween.To(() => virtualPositoin.z, SetAxisDistance, targetDistance, zoomDuration).SetEase(Ease.OutQuint);
  }

  private Tween GetRotateTween() {
    var unit = 360f / MAX_ROTATE_LEVEL;
    var targetAngle = rotateLevel * unit;
    return DOTween.To(() => yawAxisAngle, SetYawAxisAngle, targetAngle, rotateDuration).SetEase(Ease.OutQuint);
  }

  private void ChangeZoomLevel(int delta) {
    zoomLevel += delta;
    zoomLevel = (int)Mathf.Clamp(zoomLevel, zoomRange.x, zoomRange.y);
  }

  private Vector3 GetFocusPoint() {
    var dir = targetTransform.forward;
    var cos = Vector3.Dot(dir, Vector3.down);
    var h = targetTransform.position.y - focusHeight;
    var delta = dir * h / cos;
    return targetTransform.position + delta;
  }
}