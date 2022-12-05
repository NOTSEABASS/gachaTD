using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GachaPoper : MonoBehaviour {
    private Transform[] seats;
    public int SeatCount => seats.Length;
    // Start is called before the first frame update
    void Start() {
      seats = GetComponentsInChildren<Transform>().ToArray();
    }

    public void PopGacha() {
      // if (isFullyOccupied) return;
      GameObject gachaObj = null;
      int i;
      for (i = 1; i < SeatCount; i++) {
        bool occupied = MapGrid.Instance.GridNotNull(MapGrid.Instance.WorldToXZCell(seats[i].position));
        if (!occupied) {
          gachaObj = GachaFactory.Instance.GachaGenerator();
          gachaObj.transform.position = seats[i].position;
          break;
        }
      }
      if (i >= SeatCount) return;
      UniqueTween moveUniqueTween = new UniqueTween();
      var tween = gachaObj.transform.DOMoveY(.5f,0.3f);
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
