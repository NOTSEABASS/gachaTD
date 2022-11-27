using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.PlayerLoop;
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
  public GameObject GachaGenerator() {
    var tag = _randomGeneratorUtil.GetRandomMetaData();
    switch (tag) {
      case "A" :
        return GenerateGachaObject(0);
      case "B" :
        return GenerateGachaObject(1);
      case "C" :
        return GenerateGachaObject(2);
      case "D" :
        return GenerateGachaObject(3);
      case "E" :
        return GenerateGachaObject(4);
    }

    return null;
  }

  private GameObject GenerateGachaObject(int _index) {
    GameObject gachaObj = Instantiate(testResources[_index], new Vector3(0, 0, 0), Quaternion.identity);
    return gachaObj;
  }

  private void GenerateObjectInGrid(int _index) {
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
