using DG.Tweening;
using UnityEngine;

public class DraggableObject : MonoBehaviour, IOnLeftMouseDown, IOnMouseExecuting {
  #region Inner Class
  public interface IOnDragHandler {
    void OnDrag();
    void OnDragEnd();
  }

  public abstract class PositionHandler {
    private DraggableObject lifeRef;
    public float stackHeight => lifeRef != null ? lifeRef.stackHeight : 0;

    public Transform transform => lifeRef != null ? lifeRef.transform : null;

    public void SetLifeRef(DraggableObject draggable) {
      lifeRef = draggable;
    }

    public bool IsAlive() {
      return lifeRef != null;
    }

    public abstract void OnMouseDrag(Vector2 mousePosition);
    public abstract Vector3 GetPlacePosition();
  }
  #endregion

  private const float MOVE_DURATION = 0.5f;

  [SerializeField, Min(0)]
  protected float stackHeight;

  private IOnDragHandler[] onDragHandlers;
  private UniqueTween moveUniqueTween = new UniqueTween();
  private PositionHandler positionHandler;


  private void Awake() {
    LifeCollector<DraggableObject>.AddObject(this);
    onDragHandlers = GetComponents<IOnDragHandler>();
  }

  public void SetPositionHandler(PositionHandler positionHandler) {
    positionHandler.SetLifeRef(this);
    this.positionHandler = positionHandler;
  }

  private void Drag(Vector2 mousePosition) {
    foreach (var handler in onDragHandlers.SafeUObjects()) {
      handler.OnDrag();
    }
    positionHandler.OnMouseDrag(mousePosition);
    moveUniqueTween.SetAndPlay(GetMoveTween(GetDraggingPosition()), finishLastOne: false);
  }

  private void Place() {
    foreach (var handler in onDragHandlers.SafeUObjects()) {
      handler.OnDragEnd();
    }
    moveUniqueTween.SetAndPlay(GetMoveTween(GetPlacePosition()), finishLastOne: false);
  }

  private Vector3 GetDraggingPosition() {
    return GetPlacePosition();
  }

  private Vector3 GetPlacePosition() {
    return positionHandler.GetPlacePosition();
  }

  private Tween GetMoveTween(Vector3 position) {
    return transform.DOLocalMove(position, MOVE_DURATION).SetEase(Ease.OutQuint);
  }

  public MouseResult OnLeftMouseDown(MouseInputArgument arg) {
    return MouseResult.Executing | MouseResult.BreakBehind;
  }

  public MouseResult OnMouseExecuting(MouseInputArgument arg) {
    if (positionHandler == null) {
      Debug.LogError("have no valid position handler");
      return MouseResult.None;
    }

    if (arg.leftState == MouseInput.MouseState.MouseUp) {
      Place();
      return MouseResult.None;
    }
    Drag(arg.mousePosition);
    return MouseResult.Executing;
  }

}