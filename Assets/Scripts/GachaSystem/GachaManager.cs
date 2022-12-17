using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaManager : MonoSingleton<GachaManager> {
  private const int gachaPrice = 160;

  [SerializeField]
  private GachaZone gachaZone;
  public void OnSingleGachaTrigger() {
    if (TokenManager.Instance == null) {
      return;
    }
    var playerData = PlayerDataManager.PlayerData;
    if (playerData.gachaToken > gachaPrice &&
        gachaZone.CheckGachable()) {
      playerData.gachaToken += -gachaPrice;
      PlayerDataManager.PlayerData = playerData;

      var unbox = GachaFactory.Instance.GetGachaBox();
      gachaZone.PlaySingleGacha(unbox);
    }
  }

  public void OnTenGachaTrigger() {
    if (TokenManager.Instance == null) {
      return;
    }
    var playerData = PlayerDataManager.PlayerData;
    if (playerData.gachaToken > 10 * gachaPrice &&
        gachaZone.CheckGachable()) {
      playerData.gachaToken += -10 * gachaPrice;
      PlayerDataManager.PlayerData = playerData;

      var unboxes = new List<UnboxController>();
      for (int i = 0; i < 10; i++) {
        var unbox = GachaFactory.Instance.GetGachaBox();
        unboxes.Add(unbox);
      }
      gachaZone.PlayMultiyGacha(unboxes);
    }
  }
}
