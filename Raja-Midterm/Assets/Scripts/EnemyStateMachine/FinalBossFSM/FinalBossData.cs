using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FinalBossData", menuName = "FinalBossData", order = 0)]
public class FinalBossData : ScriptableObject {
    public float speed = 5f;
    public float attackDamage = 20f;
    public float attackRadius = 7f;

    public float playerDetectionRadius = 5f;
    public LayerMask whatIsPlayer;
}
