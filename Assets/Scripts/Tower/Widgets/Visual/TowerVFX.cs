using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerVFX : MonoBehaviour, ITowerLifeHandler {
  public void OnTowerDie() {
    gameObject.SetActive(false);
  }

  public void OnTowerLifeReset() {
    gameObject.SetActive(true);
  }
}
