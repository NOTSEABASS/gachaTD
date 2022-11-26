using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class GachaFactory : MonoSingleton<GachaFactory> {
  private GachaRandomGenerator _randomGeneratorUtil;
  [SerializeField] private List<GameObject> testResources;

  private void Start() {
    _randomGeneratorUtil = new GachaRandomGenerator();
    //Test Sources
    Dictionary<string, int> test = new Dictionary<string, int>{{"A",60},{"B",20},{"C",14},{"D",5},{"E",1}};
    _randomGeneratorUtil.ConsturctDataFromDict(test);
  }
  //Test Funtion
  public void GachaGenerator() {
    var tag = _randomGeneratorUtil.GetRandomMetaData();
    switch (tag) {
      case "A" :
        GenrateObject(0);
        break;
      case "B" :
        GenrateObject(1);
        break;
      case "C" :
        GenrateObject(2);
        break;
      case "D" :
        GenrateObject(3);
        break;
      case "E" :
        GenrateObject(4);
        break;
    }
   
  }

  private void GenrateObject(int _index) {
    GameObject newTower = Instantiate(testResources[_index], new Vector3(0, 0, 0), Quaternion.identity);
    UniqueTween moveUniqueTween = new UniqueTween();
    var targetPos = new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1));
    int radius = 1;
    while (MapGrid.Instance.GridNotNull(MapGrid.Instance.WorldToXZCell(targetPos))) {
      targetPos = new Vector3(Random.Range(-radius, radius), 0, Random.Range(-radius, radius));
      radius++;
    }
    var tween = newTower.transform.ThrowTo(targetPos + new Vector3(0.5f,0,0.5f), 0.3f);
    moveUniqueTween.SetAndPlay(tween);
    tween.OnComplete(() => {
      var draggableObject = newTower.GetComponent<DraggableObject>();
      if (draggableObject == null) {
        Debug.LogError("DraggableObject Component Not Found");
        return;
      }
      //Recalculate position on grid
      draggableObject.RePlace();
    });
  }
}
