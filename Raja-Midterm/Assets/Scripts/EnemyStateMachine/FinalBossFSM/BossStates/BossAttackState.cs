using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackState : BossState {
    public BossAttackState(FinalBoss boss, BossStateMachine stateMachine, FinalBossData bossData, string animBoolName) : base(boss, stateMachine, bossData, animBoolName) {
    }

    public override void AnimationTrigger() {
        base.AnimationTrigger();

        Collider2D collider = Physics2D.OverlapCircle(boss.AttackPosition, bossData.attackRadius, bossData.whatIsPlayer);

        collider.GetComponent<IDamageable>()?.TakeDamage(bossData.attackDamage);
    }

    public override void AnimationFinishTrigger() {
        base.AnimationFinishTrigger();
        stateMachine.ChangeState(boss.BigWalkState);
    }

}
