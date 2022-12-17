using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MyBox;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class GachaFactory : MonoSingleton<GachaFactory> {
  private GachaRandomGenerator _randomGeneratorUtil;
  [SerializeField]
  private UnboxController unboxPrefab;
  [SerializeField]
  private List<GameObject> tier1;
  [SerializeField]
  private List<GameObject> tier2;
  [SerializeField]
  private List<GameObject> tier3;
  private void Start() {
    _randomGeneratorUtil = new GachaRandomGenerator();
    //Test Sources
    Dictionary<string, int> test = new Dictionary<string, int> { { "C", 60 }, { "B", 20 }, { "A", 5 } };
    _randomGeneratorUtil.ConsturctDataFromDict(test);
  }


  public UnboxController GetGachaBox() {
    var unbox = Instantiate(unboxPrefab);
    var content = GachaContentGenerator();
    unbox.SetContent(content);
    return unbox;
  }
  //Test Funtion
  public GameObject GachaContentGenerator() {
    var tag = _randomGeneratorUtil.GetRandomMetaData();
    switch (tag) {
      case "A":
        return GenerateGachaObject(tier1);
      case "B":
        return GenerateGachaObject(tier2);
      case "C":
        return GenerateGachaObject(tier3);
    }

    return null;
  }

  private GameObject GenerateGachaObject(List<GameObject> prefabs) {
    var prefab = prefabs.GetRandom();
    GameObject gachaObj = Instantiate(prefab);
    return gachaObj;
  }



}
