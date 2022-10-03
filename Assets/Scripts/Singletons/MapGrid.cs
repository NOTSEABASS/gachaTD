using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class MapGrid : MonoSingleton<MapGrid> {
  #region Inner Classes
  private class Cell {
    public List<DraggablePositionHandler> draggableHandlers = new List<DraggablePositionHandler>();
  }

  private class CollectorPlugin : LifeCollector<DraggableObject>.Plugin {
    private MapGrid mapGrid;
    public CollectorPlugin(MapGrid mapGrid) : base(mapGrid) {
      this.mapGrid = mapGrid;
    }
    public override void OnAddObject(DraggableObject obj) {
      mapGrid.OnAddDraggable(obj);
    }
  }

  private class DraggablePositionHandler : DraggableObject.PositionHandler {
    private MapGrid mapGrid;
    public Vector2Int cell;

    public DraggablePositionHandler(MapGrid mapGrid) {
      this.mapGrid = mapGrid;
    }

    public void Init(DraggableObject draggable) {
      cell = mapGrid.WorldToXZCell(draggable.transform.position);
    }

    public override void OnMouseDrag(Vector2 mousePosition) {
      if (mapGrid.MouseRaycastCell(mousePosition, out var newCell) && newCell != cell) {
        var curCellData = mapGrid.GetCellDataNotNull(cell);
        curCellData.draggableHandlers.Remove(this);

        var newCellData = mapGrid.GetCellDataNotNull(newCell);
        newCellData.draggableHandlers.Add(this);

        cell = newCell;
      }
    }

    public override Vector3 GetPlacePosition() {
      var cellData = mapGrid.GetCellDataNotNull(cell);
      var height = 0f;
      for (int i = 0, count = cellData.draggableHandlers.SafeCount(); i < count; i++) {
        if (cellData.draggableHandlers[i] == this) {
          break;
        }
        height += 1; //todo: dynamic height
      }

      var result = mapGrid.XZCellToWorld(cell).SetY(height);
      return result;
    }

  }

  #endregion


  [SerializeField]
  private Grid grid;

  private IGrid<Cell> gridData = new DictionaryGridImpl<Cell>();

  protected override void Awake() {
    base.Awake();
    LifeCollector<DraggableObject>.SetPlugin(new CollectorPlugin(this));
  }

  private bool MouseRaycastCell(Vector2 mousePosition, out Vector2Int cell) {
    cell = Vector2Int.zero;

    var layerMask = LayerMask.GetMask("MapGrid");
    var ray = Camera.main.ScreenPointToRay(mousePosition);
    if (Physics.Raycast(ray, out RaycastHit hitInfo, 9999f, layerMask)) {
      var hitPosition = hitInfo.point;

      cell = WorldToXZCell(hitPosition);

      return true;
    }
    return false;
  }



  protected Vector3 XZCellToWorld(Vector2Int xzCell) {
    var offset = grid.CellToWorld(new Vector3Int(1, 0, 1)) * 0.5f; // offset to grid center, not considering Y
    var result = grid.CellToWorld(Vector3Int.zero.SetXZ(xzCell)) + offset;
    return result;
  }

  protected Vector2Int WorldToXZCell(Vector3 world) {
    var result = grid.WorldToCell(world);
    return result.XZ();
  }

  protected void OnAddDraggable(DraggableObject draggable) {
    var positionHandler = new DraggablePositionHandler(this);
    positionHandler.Init(draggable);

    var cellData = GetCellDataNotNull(positionHandler.cell);
    cellData.draggableHandlers.Add(positionHandler);

    draggable.SetPositionHandler(positionHandler);
    draggable.transform.position = positionHandler.GetPlacePosition();
  }

  private Cell GetCellDataNotNull(Vector2Int position) {
    if (!gridData.Contains(position)) {
      gridData[position] = new Cell();
    }
    return gridData[position];
  }

}
