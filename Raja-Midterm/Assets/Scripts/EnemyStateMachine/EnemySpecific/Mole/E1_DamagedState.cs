using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class E1_DamagedState : DamagedState {
    private Enemy1 enemy;
    private float damageTime = 0.5f;
    private float countDown;
    public E1_DamagedState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Enemy1 enemy) : base(entity, stateMachine, animBoolName) {
        this.enemy = enemy;
    }
    public override void Enter() {
        base.Enter();
        enemy.StateMachine.preventStateChange = true;
        countDown = damageTime;

    }

    public override void LogicUpdate() {
        base.LogicUpdate();
        if (isAnimationFinished) {
            stateMachine.ChangeState(enemy.lookForPlayerState);
        }
        Timer();
        // Debug.Log(isAnimationFinished);

    }

    private void Timer() {

        if (countDown >= 0) {
            countDown -= Time.deltaTime;
            return;
        } else {
            enemy.StateMachine.preventStateChange = false;
            FinishDamaged();
        }


        // isAnimationFinished = true;
        // isTakingDamage = false;
    }


}
