using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Detector {
  private const int DEFAULT_DETECT_INTERVAL = 2;
  private int intervalCounter;
  protected int detectInterval = DEFAULT_DETECT_INTERVAL;

  private DetectResult cachedResult;
  public DetectResult Detect(DetectParam param) {
    if (intervalCounter == 0) {
      cachedResult = DoDetect(param);
    }
    intervalCounter++;
    intervalCounter %= detectInterval;

    return cachedResult;
  }

  protected abstract DetectResult DoDetect(DetectParam param);
}
