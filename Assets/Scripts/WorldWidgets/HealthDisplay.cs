using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthDisplay : MonoBehaviour {
  [SerializeField]
  private TMP_Text healthText;

  private PlayerData cachedData;

  private void Update() {
    if (cachedData.CheckUpdate()) {
      healthText.text = cachedData.coreHealth.ToString("0##");
    }
  }
}
