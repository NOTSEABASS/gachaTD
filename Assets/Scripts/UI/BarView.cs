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
  private RectTransform scale;

  public void Render(Model model) {
    float normalized;
    if (model.max <= 0) {
      normalized = 0;
    } else {
      normalized = (float)model.current / model.max;
    }
    if (currentText != null) {
      currentText.text = $"{model.current} / {model.max}";
    }
    if (scale != null) {
      scale.DOScaleX(normalized, 0.17f).Play();
    }
  }



}
