using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnemyBoidManager : MonoSingleton<EnemyBoidManager> {
  [SerializeField]
  private float k = 0.1f;
  [SerializeField]
  private float p = 2;
  [SerializeField]
  private float max = 10;
  private List<EnemyBoidIdentity> identities = new List<EnemyBoidIdentity>();

  public void FixedUpdate() {
    identities.RemoveAll(x => !x.isValid);
    foreach (var identity in identities) {
      Vector3 force = Vector3.zero;
      var position = identity.position;
      foreach (var otherIdentity in identities) {
        var rand = Random.value;
        if (rand > 0.5f) {
          continue;
        }
        if (otherIdentity == identity) {
          continue;
        }
        force += ComputeBoidForceTo(position, otherIdentity.position);
      }
      identity.boidForce = force;
    }
  }

  public void AddIdentity(EnemyBoidIdentity identity) {
    identities.Add(identity);
  }

  public Vector3 ComputeBoidForceTo(Vector3 selfPosition, Vector3 otherPosition) {
    var dir = selfPosition - otherPosition;
    var distance = Vector3.Distance(otherPosition, selfPosition);
    return Mathf.Min(max, k / Mathf.Pow(distance, p)) * dir.normalized;
  }
}

public class EnemyBoidIdentity {
  public bool isValid;
  public Vector3 boidForce;
  public Vector3 position => transform.position;
  private Transform transform;

  public EnemyBoidIdentity(Transform transform) {
    this.transform = transform;
    EnemyBoidManager.Instance.AddIdentity(this);
  }
}
