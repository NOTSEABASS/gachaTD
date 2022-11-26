using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedClock {
  public float freq {
    set {
      if (value > 0) {
        m_interval = 1 / value;
      } else {
        m_interval = float.PositiveInfinity;
      }
    }
  }

  public float interval {
    set {
      m_interval = Mathf.Max(0, m_interval);
    }
  }

  private float m_interval;
  private float m_counter;

  public float normalizedTime => Mathf.Clamp01(m_counter / Mathf.Max(m_interval, 0.0001f));

  public void SetInterval(float interval) {
    this.m_interval = interval;
  }

  public void Update(float deltaTime) {
    m_counter += deltaTime;
  }

  public void OnTrigger() {
    m_counter = 0;
  }

  public bool IsReady() {
    return m_counter >= m_interval;
  }


}
