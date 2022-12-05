using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Rendering;

public class DraggingMountManager : MonoSingleton<DraggingMountManager> {
  #region Inner Classes
  private class CollectorPlugin : LifeCollector<DraggableObject>.Plugin {
    private DraggingMountManager dmm;
    public CollectorPlugin(DraggingMountManager mapGrid) : base(mapGrid) {
      this.dmm = mapGrid;
    }
    public override void OnAddObject(DraggableObject obj) {
      dmm.OnAddDraggable(obj);
    }
  }


  public class DraggablePositionHandler : DraggableObject.PositionHandler {
    private static readonly string[] rayCastLayers = { "DraggingMount" };
    private Vector3 placePosition;

    private Transform startMount;
    private Vector3 startPosition;

    private Transform cachedMount;
    public override Vector3 GetPlacePosition() {
      return placePosition;
    }
    public override void OnMouseStartDrag() {
      startMount = transform.parent;
      startPosition = startMount ? transform.localPosition : Vector3.zero;
      transform.parent = null;
    }

    public override void OnMouseStopDrag() {
      if (cachedMount != null) {
        transform.parent = cachedMount;
        placePosition = Vector3.zero;
      } else {
        transform.parent = startMount;
        placePosition = startPosition;
      }
    }

    public override void OnMouseDrag(Vector2 mousePosition) {
      var ray = Camera.main.ScreenPointToRay(mousePosition);
      var planePoint = Vector3.zero;
      if (cachedMount != null) {
        planePoint = cachedMount.position;
      } else {
        planePoint = startPosition;
      }
      var plane = new Plane(Vector3.up, startPosition);
      if (plane.Raycast(ray, out var d)) {
        placePosition = ray.GetPoint(d);
      }

      var mask = LayerMask.GetMask(rayCastLayers);
      if (Physics.Raycast(ray, out var hitInfo, 9999, mask, QueryTriggerInteraction.Collide) &&
          hitInfo.transform.childCount == 0) {
        cachedMount = hitInfo.transform;
      } else {
        cachedMount = null;
      }
    }
    public override void Recalculate() {

    }

    #endregion
  }
  private static readonly string[] rayCastLayers = { "MapGrid", "DraggableObject" };

  protected override void Awake() {
    base.Awake();
    LifeCollector<DraggableObject>.SetPlugin(new CollectorPlugin(this));
  }

  public void OnAddDraggable(DraggableObject obj) {
    obj.SetPositionHandler(new DraggablePositionHandler());
  }

}