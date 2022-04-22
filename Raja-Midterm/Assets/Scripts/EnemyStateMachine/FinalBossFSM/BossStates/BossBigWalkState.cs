using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBigWalkState : BossState {
    private bool isRight;
    private bool isPlayerInRadius;
    public BossBigWalkState(FinalBoss boss, BossStateMachine stateMachine, FinalBossData bossData, string animBoolName) : base(boss, stateMachine, bossData, animBoolName) {
    }

    public override void DoChecks() { // fixed update
        base.DoChecks();
        isRight = Player.Instance.transform.position.x > boss.transform.position.x;
        isPlayerInRadius = boss.CheckIfPlayerInRadius();

    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
        if (isPlayerInRadius) stateMachine.ChangeState(boss.AttackState);
    }

    public override void LogicUpdate() {
        base.LogicUpdate();
        var dir = isRight ? Vector2.right : Vector2.left;
        boss.SetCurrentVelocity(dir * bossData.speed);
    }
}
