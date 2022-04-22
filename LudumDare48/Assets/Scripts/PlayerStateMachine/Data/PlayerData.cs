using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Player Data/Base Player Data")]
public class PlayerData : ScriptableObject {

    [Header("Move State")]
    public float movementVelocity = 5f;
    public float groundCheckRadius = 0.3f;
    public float wallCheckDistance = 0.5f;
    public LayerMask whatIsGround;

    [Header("Jump State")]
    public float jumpVelocity = 23f;
    public int amountOfJumps = 1;

    [Header("Air State")]
    public float coyoteTime = 0.2f;
    public float fallMulitiplier = 5f;
    public float lowJumpMulitiplier = 10f;


}
