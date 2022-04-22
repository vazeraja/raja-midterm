using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBossFight : MonoBehaviour {
    // Start is called before the first frame update
    // Update is called once per frame
    void Update() {
        if (Player.Instance.transform.position.x > transform.position.x) {
            FinalBossController.Instance.StartBossFight();
        }
    }
}
