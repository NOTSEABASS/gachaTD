using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.Remoting.Channels;

public class TowerPropertyView : MonoBehaviour {
  public enum PropertyObject {
    AttackValue,
    AttackFreq,
    AttackRadius,
    EnergyRecover,
  }

  [SerializeField]
  private TMP_Text propertyName;
  [SerializeField]
  private TMP_Text propertyValue;

  public void Clear() {
    propertyName.text = "";
    propertyValue.text = "";
  }

  public void Render(PropertyObject property, float value) {
    switch (property) {
      case PropertyObject.AttackValue:
        propertyName.text = "攻击力";
        propertyValue.text = ((int)value).ToString();
        break;
      case PropertyObject.AttackFreq:
        propertyName.text = "攻击速度";
        propertyValue.text = $"{value.ToString("0.0")} 次/秒";
        break;
      case PropertyObject.AttackRadius:
        propertyName.text = "攻击距离";
        propertyValue.text = $"{value.ToString("0.0")} 码";
        break;
      case PropertyObject.EnergyRecover:
        propertyName.text = "能量回复";
        propertyValue.text = $"{value.ToString("0.#")} 点/秒";
        break;
      default:
        break;
    }
  }
}
