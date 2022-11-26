using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEngine : MonoSingleton<BattleEngine> {

  private List<System> systems = new List<System>();
  private Queue<Event> eventBuffer = new Queue<Event>();

  public void PushEvent(Event eve) {
    eventBuffer.Enqueue(eve);
  }

  public void AddSystem(System system) {
    SetupSystem(system);
    systems.Add(system);
  }

  public void OnResolve() {
    while (eventBuffer.Count > 0) {
      var eve = eventBuffer.Dequeue();
      foreach (var sys in systems) {
        sys.Handle(eve);
      }
    }
  }

  private void SetupSystem(System system) {
    system.enemyDataHub = (EnemyDataHub)EnemyDataHub.Instance;
    system.towerDataHub = (TowerDataHub)TowerDataHub.Instance;
  }

  protected override void Awake() {
    base.Awake();
    AddSystem(new TowerDamageSystem());
    AddSystem(new EnemyDeathSystem());
  }

  private void Update() {
    OnResolve();
  }

  public abstract class Event {

  }

  public abstract class System {
    public EnemyDataHub enemyDataHub;
    public TowerDataHub towerDataHub;

    public abstract void Handle(Event eve);
  }
}
