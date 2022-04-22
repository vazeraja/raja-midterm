using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLittleWalkState : BossState {
    public BossLittleWalkState(FinalBoss boss, BossStateMachine stateMachine, FinalBossData bossData, string animBoolName) : base(boss, stateMachine, bossData, animBoolName) {
    }

    public override void LogicUpdate() {
        base.LogicUpdate();
        boss.SetCurrentVelocity(Vector2.right * bossData.speed);

        if (boss.transform.position.x > boss.CheckPoints[boss.CheckPtIndex].transform.position.x) {
            stateMachine.ChangeState(boss.IdleState);
            boss.CheckPtIndex++;
        }
    }
}
