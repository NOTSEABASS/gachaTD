using UnityEngine;

public class DraggableObject : MouseInputHandlerBase {
  private bool isDragging = false;

  private void Start() {

  }

  void Update() {
    if (!isDragging) {
      return;
    }

  }

  private void Place() {
    print("Place");
  }

  public override MouseResult OnLeftMouseDown(MouseInputArgument arg) {
    isDragging = true;
    return MouseResult.Executing;
  }

  public override MouseResult OnMouseExecuting(MouseInputArgument arg) {
    if (arg.leftState == MouseInput.MouseState.MouseUp) {
      isDragging = false;
      Place();
      return MouseResult.None;
    }
    return MouseResult.Executing;
  }

}
