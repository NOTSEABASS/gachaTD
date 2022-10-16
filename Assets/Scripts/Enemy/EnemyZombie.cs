using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZombie : EnemyBase, IOnRightMouseDown {
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public MouseResult OnRightMouseDown(MouseInputArgument arg) {
        Debug.Log("Test Release");
        // Debug.Log(polc.poolName);
        EnemyPoolManager.Instance.Release(polc.poolName,gameObject);
        return MouseResult.BreakBehind;
    }
}
