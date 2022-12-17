using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PrimongemView : MonoBehaviour {
  [SerializeField]
  private TMP_Text countText;
  private PlayerData cachedData;

  // Update is called once per frame
  void Update() {
    if (cachedData.CheckUpdate()) {
      countText.text = cachedData.gachaToken.ToString();
    }
  }
}
