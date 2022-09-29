using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class MapGrid : MonoSingleton<MapGrid> {
  #region Inner Classes
  private class Cell {

  }
  #endregion
  [SerializeField]
  private Grid grid;
  private IGrid<Cell> dataGrid = new ArrayGridImpl<Cell>();


  public bool MouseRaycastAlignedPosition(Vector2 mousePosition, out Vector3 res) {
    res = Vector3.zero;

    var layerMask = LayerMask.GetMask("MapGrid");
    var ray = Camera.main.ScreenPointToRay(mousePosition);
    if (Physics.Raycast(ray, out RaycastHit hitInfo, 9999f, layerMask)) {
      res = Align(grid, hitInfo.point);
      return true;
    }
    return false;
  }

  private Vector3 Align(Grid grid, Vector3 position) {
    var offset = grid.CellToWorld(Vector3Int.one) * 0.5f; // offset to grid center
    return grid.CellToWorld(grid.WorldToCell(position)) + offset;
  }
}
