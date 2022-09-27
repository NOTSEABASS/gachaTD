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
  private float focusHeight;
  private Transform targetTransform => targetCamera.transform;

  private float originDistance;
  private Vector3 originFlattenForward;

  private const int MAX_ROTATE_LEVEL = 4; // hard-coded 4 direction

  private float axisDistance;
  private float yawAxisAngle;

  private UniqueTween moveUniqueTween = new UniqueTween();
  private UniqueTween rotateUniqueTween = new UniqueTween();

  private void Start() {
    originFlattenForward = targetTransform.forward;
    originFlattenForward.y = 0;
    originDistance = GetOriginAxisDistance();
    UpdateCameraPosition();
  }

  void Update() {
    var scrollInput = Input.GetAxis("Mouse ScrollWheel");
    if (scrollInput > 0) {
      ChangeZoomLevel(-1);
      UpdateCameraPosition();
      return;
    }
    if (scrollInput < 0) {
      ChangeZoomLevel(+1);
      UpdateCameraPosition();
      return;
    }


    if (Input.GetKeyDown(KeyCode.Q)) {
      rotateLevel = (rotateLevel + 1); //没有求余，因为会导致360-0的边界错误，所以没做
      UpdateCameraRotation();
      return;
    }
    if (Input.GetKeyDown(KeyCode.E)) {
      rotateLevel = (rotateLevel - 1);
      UpdateCameraRotation();
      return;
    }
  }

  private void FixedUpdate() {
    var verticalInput = Input.GetAxis("Vertical");
    var horizontalInput = Input.GetAxis("Horizontal");
    var direction = new Vector3(targetTransform.forward.x, 0, targetTransform.forward.z) * verticalInput;
    direction += new Vector3(targetTransform.right.x, 0, targetTransform.right.z) * horizontalInput;
    targetTransform.localPosition += direction.normalized * Time.deltaTime * moveSpeed;
  }

  private void UpdateCameraPosition() {
    moveUniqueTween.SetAndPlay(GetMoveTween(), finishLastOne: false);
  }

  private void UpdateCameraRotation() {
    rotateUniqueTween.SetAndPlay(GetRotateTween(), finishLastOne: false);
  }

  private float GetOriginAxisDistance() {
    var mat = targetTransform.worldToLocalMatrix;
    var originInLocal = mat.MultiplyPoint(GetFocusPoint());
    return originInLocal.z;
  }

  private void SetAxisDistance(float d) {
    axisDistance = d;
    RecaculatePositionAndRotation();
  }

  private void SetYawAxisAngle(float deg) {
    yawAxisAngle = deg;
    RecaculatePositionAndRotation();
  }

  private void RecaculatePositionAndRotation() {
    var mat = targetTransform.worldToLocalMatrix;
    var focusPoint = GetFocusPoint();
    var originInLocal = mat.MultiplyPoint(focusPoint);
    var newLocalPosition = new Vector3(0, 0, originInLocal.z - axisDistance);
    targetTransform.position = targetTransform.localToWorldMatrix.MultiplyPoint(newLocalPosition);

    var flattenForward = targetTransform.forward;
    flattenForward.y = 0;

    var targetFlatten = Quaternion.AngleAxis(yawAxisAngle, Vector3.up) * originFlattenForward;
    var delta = Vector3.SignedAngle(flattenForward, targetFlatten, Vector3.up);

    targetTransform.RotateAround(focusPoint, Vector3.up, delta);
  }

  private Tween GetMoveTween() {
    var d = zoomUnitLength * zoomLevel + originDistance;
    return DOTween.To(() => axisDistance, SetAxisDistance, d, zoomDuration).SetEase(Ease.OutQuint);
  }

  private Tween GetRotateTween() {
    var unit = 360f / MAX_ROTATE_LEVEL;
    var angle = rotateLevel * unit;
    return DOTween.To(() => yawAxisAngle, SetYawAxisAngle, angle, rotateDuration).SetEase(Ease.OutQuint);
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