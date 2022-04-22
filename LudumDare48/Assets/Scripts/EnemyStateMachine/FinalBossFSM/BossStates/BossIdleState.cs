using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleState : BossState {
    public BossIdleState(FinalBoss boss, BossStateMachine stateMachine, FinalBossData bossData, string animBoolName) : base(boss, stateMachine, bossData, animBoolName) {
    }

    public override void Enter() {
        base.Enter();
        boss.SetCurrentVelocity(Vector2.zero);
    }

    public override void LogicUpdate() {
        base.LogicUpdate();
        if (Player.Instance.transform.position.x > boss.transform.position.x) {
            stateMachine.ChangeState(boss.LittleWalkState);
        }
        if (Player.Instance.transform.position.x > boss.CheckPoints[boss.CheckPoints.Count - 1].transform.position.x) {
            stateMachine.ChangeState(boss.TransformationState);
        }

    }


}
