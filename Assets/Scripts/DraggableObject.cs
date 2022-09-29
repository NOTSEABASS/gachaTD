using DG.Tweening;
using UnityEngine;

public class DraggableObject : MonoBehaviour, IOnLeftMouseDown, IOnMouseExecuting {
  private bool isDragging = false;

  private void Start() {

  }

  void Update() {
    if (!isDragging) {
      return;
    }
    Drag();
  }

  private void Drag() {
    Vector3 targetPos = Vector3.zero;
    MapGrid.Instance.MouseRaycastAlignedPosition(Input.mousePosition, out targetPos);
    targetPos = targetPos.SetY(transform.position.y);
    // Debug.Log(targetPos);
    if ((transform.position - targetPos).magnitude > 0.1f) {
      transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 10);
    }
  }

  private void Place() {
    print("Place");
    Vector3 targetPos = Vector3.zero;
    MapGrid.Instance.MouseRaycastAlignedPosition(Input.mousePosition, out targetPos);
    transform.position = targetPos;
  }

  public MouseResult OnLeftMouseDown(MouseInputArgument arg) {
    isDragging = true;
    return MouseResult.Executing;
  }

  public  MouseResult OnMouseExecuting(MouseInputArgument arg) {
    if (arg.leftState == MouseInput.MouseState.MouseUp) {
      isDragging = false;
      Place();
      return MouseResult.None;
    }
    return MouseResult.Executing;
  }

}
