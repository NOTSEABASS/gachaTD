using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputForwarder : MonoBehaviour {
  [SerializeField]
  private GameObject forwardTarget;
  public GameObject ForwardTarget => forwardTarget;
}
