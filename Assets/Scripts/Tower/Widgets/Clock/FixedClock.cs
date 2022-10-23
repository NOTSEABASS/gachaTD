using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedClock {
  public float interval;
  private float counter;

  public void OnFixedUpdate() {
    if (counter >= 0) {
      counter -= Time.fixedDeltaTime;
    }
  }

  public void Reset() {
    counter = interval;
  }

  public bool IsReady() {
    return counter < 0;
  }

  public float GetPercentage() {
    return 1 - Mathf.Clamp01(counter / interval);
  }

}
