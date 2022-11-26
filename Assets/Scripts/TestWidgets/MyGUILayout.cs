using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidgetGUILayout : MonoSingleton<WidgetGUILayout> {
  private const int BUTTON_HEIGHT = 60;
  private const int BUTTON_WIDTH = 150;
  private const int PADDING = 5;

  private Vector2Int topLeftButtonPos = new Vector2Int(10, 10);
  private Dictionary<string, int> buttonHashToIndex = new Dictionary<string, int>();
  private int index;

  private int GetButtonIndex(string hash) {
    if (!buttonHashToIndex.ContainsKey(hash)) {
      buttonHashToIndex[hash] = index++;
    }
    return buttonHashToIndex[hash];
  }

  private bool _Button(string text, string hash) {
    var pos = topLeftButtonPos;
    pos.y += GetButtonIndex(hash) * (BUTTON_HEIGHT + PADDING);
    var res = GUI.Button(new Rect(pos, new Vector2Int(BUTTON_WIDTH, BUTTON_HEIGHT)), text);
    return res;
  }

  public static bool Button(string text, string hash) {
    return InstanceNotNull._Button(text, hash);
  }
}
