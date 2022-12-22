using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
[ExecuteAlways]
public class FaceToCamera : MonoBehaviour {
  void Update() {
    if (!Application.isPlaying && Camera.current != null) {
      FaceTo(Camera.current);
    } else if (Application.isPlaying) {
      FaceTo(Camera.main);
    }
  }

  private void FaceTo(Camera camera) {
    transform.up = camera.transform.up;
    transform.forward = -camera.transform.forward;
  }
}
