using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTransformationState : BossState {
    public BossTransformationState(FinalBoss boss, BossStateMachine stateMachine, FinalBossData bossData, string animBoolName) : base(boss, stateMachine, bossData, animBoolName) {
    }

    public override void Enter() {
        base.Enter();
        boss.SetCurrentVelocity(Vector2.zero);
        boss.IsInvincible = false;
    }

    public override void AnimationFinishTrigger() {
        base.AnimationFinishTrigger();
        stateMachine.ChangeState(boss.BigWalkState);
    }
}
