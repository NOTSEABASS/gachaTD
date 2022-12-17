using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoSingleton<PlayerDataManager> {
  [SerializeField]
  private PlayerData playerData;
  public static PlayerData PlayerData {
    get => Instance.playerData;
    set {
      value.UpdateVersion();
      Instance.playerData = value;
    }
  }

  public void Start() {
    playerData.UpdateVersion();
  }
}

[System.Serializable]
public struct PlayerData {
  [SerializeField]
  private int version;
  public int coreHealth;
  public int gachaToken;

  public void UpdateVersion() {
    version++;
  }

  private bool HasDiff(PlayerData data) {
    return data.version != version;
  }

  public bool CheckUpdate() {
    var data = PlayerDataManager.PlayerData;
    if (HasDiff(data)) {
      this = data;
      return true;
    }
    return false;
  }
}
