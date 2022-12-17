using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GachaPoper : MonoBehaviour {
  private List<Transform> seats = new List<Transform>();
  // Start is called before the first frame update
  void Start() {
    var children = GetComponentsInChildren<Transform>().ToArray();
    foreach (var child in children) {
      if (child.gameObject.layer == LayerConsts.DraggingMountLayer) {
        seats.Add(child);
      }
    }
  }

  public bool HasEmptySlot() {
    for (int i = 0; i < seats.Count; i++) {
      if (seats[i].childCount == 0) {
        return true;
      }
    }
    return false;
  }

  public void PopGacha() {
    // if (isFullyOccupied) return;
    GameObject gachaObj = null;
    int i;
    for (i = 0; i < seats.Count; i++) {
      bool occupied = seats[i].childCount != 0;
      if (!occupied) {
        gachaObj = GachaFactory.Instance.GachaContentGenerator();
        gachaObj.transform.position = seats[i].position;
        gachaObj.transform.parent = seats[i];
        break;
      }
    }
    if (i >= seats.Count) return;
    UniqueTween moveUniqueTween = new UniqueTween();
    gachaObj.transform.position += Vector3.down;
    var tween = gachaObj.transform.DOMoveY(.11f, 0.3f);
    moveUniqueTween.SetAndPlay(tween);
    tween.OnPlay(() => {
      var draggableObject = gachaObj.GetComponent<DraggableObject>();
      if (draggableObject == null) {
        Debug.LogError("DraggableObject Component Not Found");
        return;
      }
      //Recalculate position on grid
      draggableObject.RePlace();
    });
  }
}
