using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_PlayerDetectedState : PlayerDetectedState {

    private Enemy1 enemy;
    private float countDown;
    private MoleRockThrow rockThrow;

    public E1_PlayerDetectedState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, SO_PlayerDetected stateData, Enemy1 enemy) : base(entity, stateMachine, animBoolName, stateData) {
        this.enemy = enemy;
    }

    public override void Enter() {
        base.Enter();

        Shoot();
    }
    public override void Exit() {
        base.Exit();
    }
    public override void LogicUpdate() {
        base.LogicUpdate();

        if (countDown >= 0) countDown -= Time.deltaTime;

        // if (performCloseRangeAction) {
        //     stateMachine.ChangeState(enemy.meleeAttackState);
        // } else 
        if (performLongRangeAction) {
            stateMachine.ChangeState(enemy.chargeState);
        } else if (!isPlayerInMaxAgroRange) {
            stateMachine.ChangeState(enemy.lookForPlayerState);
        }

    }
    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }
    private void Shoot() {

        if (countDown <= 0) {
            countDown = 1f / rockThrow.FireRate;
            rockThrow.ShootBullet();
        }
    }
    public void SetWeapon(MoleRockThrow weapon) {
        this.rockThrow = weapon;
    }
}
