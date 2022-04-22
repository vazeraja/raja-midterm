using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState {
    private int xInput;
    private bool jumpInput;
    private bool jumpInputStop;

    private bool isGrounded;
    private bool isTouchingWall;
    private bool isTouchingLedge;

    private bool coyoteTime;

    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) {
    }
    public override void DoChecks() {
        base.DoChecks();

        isGrounded = player.CheckIfGrounded();
        isTouchingWall = player.CheckIfTouchingWall();
        isTouchingLedge = player.CheckIfTouchingLedge();
    }

    public override void LogicUpdate() {
        base.LogicUpdate();

        CheckCoyoteTime();

        xInput = player.InputHandler.NormInputX;
        jumpInput = player.InputHandler.JumpInput;
        jumpInputStop = player.InputHandler.JumpInputStop;

        CheckJumpMultiplier();

        if (isGrounded && player.CurrentVelocity.y < 0.01f) {
            stateMachine.ChangeState(player.LandState);
        } else if (jumpInput && player.JumpState.canJump()) {
            stateMachine.ChangeState(player.JumpState);
        } else {
            player.CheckIfShouldFlip(xInput);
            player.SetVelocityX(playerData.movementVelocity * xInput);

            player.Anim.SetFloat("yVelocity", player.CurrentVelocity.y);
            player.Anim.SetFloat("xVelocity", Mathf.Abs(player.CurrentVelocity.x));
        }
    }
    private void CheckCoyoteTime() {
        if (coyoteTime && Time.time > startTime + playerData.coyoteTime) {
            coyoteTime = false;
            player.JumpState.DecreaseAmountOfJumpsLeft();
        }
    }
    private void CheckJumpMultiplier() {
        if (player.CurrentVelocity.y < 0) {
            player.SetVelocityY(player.CurrentVelocity.y + Physics2D.gravity.y * (playerData.fallMulitiplier - 1) * Time.deltaTime);
        } else if (player.CurrentVelocity.y > 0 && jumpInputStop) {
            player.SetVelocityY(player.CurrentVelocity.y + Physics2D.gravity.y * (playerData.lowJumpMulitiplier - 1) * Time.deltaTime);
        }
    }
    public void StartCoyoteTime() => coyoteTime = true;


}
