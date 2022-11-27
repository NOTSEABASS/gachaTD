using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using System.Transactions;
using DG.Tweening;

public class BarView : MonoBehaviour {
  public struct Model {
    public int current;
    public int max;
  }
  [SerializeField]
  private TMP_Text currentText;
  [SerializeField]
  private TMP_Text maxText;
  [SerializeField]
  private RectTransform scale;

  public void Render(Model model) {
    float normalized;
    if (model.max <= 0) {
      normalized = 0;
    } else {
      normalized = (float)model.current / model.max;
    }
    if (currentText != null) {
      currentText.text = model.current.ToString();
    }
    if (maxText != null) {
      maxText.text = model.max.ToString();
    }
    if (scale != null) {
      scale.DOScaleZ(normalized, 0.17f);
    }
    StartCoroutine(Refresh());
  }

  private IEnumerator Refresh() {
    currentText.enabled = false;
    yield return null;
    currentText.enabled = true;
  }


}
