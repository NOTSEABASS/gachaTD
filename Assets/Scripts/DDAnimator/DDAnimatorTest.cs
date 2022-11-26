using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DDAnimatorTest : MonoBehaviour {
  [SerializeField]
  DDAnimator DDAnimator;

  [Range(0.1f, 2f)]
  public float atkTime;
  public float timer;
  public float transitionDuration;
  public float atkOffset;
  public float idleLoopTime;

  public FixedClock clock = new FixedClock();
  public bool isAttacking;
  public StraightFly bulletPrefab;
  public Transform launchPoint;
  public float bulletSpeed;

  // Start is called before the first frame update
  void Start() {

  }

  // Update is called once per frame
  void Update() {
    Key();


    clock.SetInterval(atkTime);

    if (Input.GetKeyUp(KeyCode.Z)) {
      isAttacking = true;
      DDAnimator.SetState("Move", atkOffset, transitionDuration); //设置攻击动画
      DDAnimator.SetUpdateMode(DDAnimator.UpdateMode.Manual); //手动更新normalized time
    }

    if (Input.GetKeyUp(KeyCode.X)) {
      isAttacking = false;
      DDAnimator.SetState("Idle", 0, transitionDuration); //设置闲置动画
      DDAnimator.SetUpdateMode(DDAnimator.UpdateMode.Loop, loopTime: idleLoopTime); //闲置状态自动更新
    }

    if (isAttacking) {
      clock.Update(Time.deltaTime);
      DDAnimator.SetNormalizedTime(clock.normalizedTime); //用逻辑计时器更新动画
      if (clock.IsReady()) {
        Launch();
        clock.OnTrigger();
      }
    } else {
      clock.OnTrigger();
    }


  }

  private void Key() {
    if (Input.GetKey(KeyCode.A)) {
      DDAnimator.SetUpdateMode(DDAnimator.UpdateMode.Manual);
      if (DDAnimator.SetState("Idle", 0, transitionDuration)) {
        timer = 0;
      }
      DDAnimator.SetNormalizedTime(timer / atkTime);
      timer += Time.deltaTime;
      timer %= atkTime;
    }
    if (Input.GetKey(KeyCode.S)) {
      DDAnimator.SetUpdateMode(DDAnimator.UpdateMode.Manual);
      if (DDAnimator.SetState("Move", atkOffset, transitionDuration)) {
        timer = 0;
      }
      DDAnimator.SetNormalizedTime(timer / atkTime);
      timer += Time.deltaTime;
      timer %= atkTime;
    }
    if (Input.GetKeyUp(KeyCode.D)) {
      DDAnimator.SetUpdateMode(DDAnimator.UpdateMode.Loop, atkTime);
      DDAnimator.SetState("Move", atkOffset, transitionDuration);
    }
  }

  private void Launch() {
    var bullet = Instantiate(bulletPrefab);
    bullet.transform.position = launchPoint.position;
    bullet.speed = bulletSpeed;
  }

}

