using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamagedState : PlayerState {
    // Start is called before the first frame update
    private bool isTakingDamage;
    private bool hitSideRight;
    private float hitForceX, hitForceY;
    public PlayerDamagedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) {
    }

    public override void AnimationFinishTrigger() {
        base.AnimationFinishTrigger();
        isTakingDamage = false;

    }

    public override void AnimationTrigger() {
        base.AnimationTrigger();
    }

    public override void Enter() {
        base.Enter();
        hitForceX = 4f;
        hitForceY = 5f;
        StartDamageAnimation();
    }


    public override void LogicUpdate() {
        base.LogicUpdate();
        if (isAnimationFinished) {
            stateMachine.ChangeState(player.IdleState);
        }
    }

    public void HitSide(bool rightSide) {
        this.hitSideRight = rightSide;
    }

    public void SetHitForce(float x, float y) {
        this.hitForceX = x;
        this.hitForceY = y;
    }

    private void StartDamageAnimation() {
        if (!isTakingDamage) {
            isTakingDamage = true;
            if (hitSideRight) hitForceX *= -1;
            player.RB.drag = 0f;
            player.RB.velocity = Vector2.zero;
            player.RB.AddForce(new Vector2(hitForceX, hitForceY), ForceMode2D.Impulse);
            // sound
        }
    }

}
