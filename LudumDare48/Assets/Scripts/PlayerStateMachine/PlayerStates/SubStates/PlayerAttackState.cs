using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerAbilityState {
    // private Weapon weapon;
    // private float countDown;
    // private int mshoot = Animator.StringToHash("mshoot");
    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) {
    }

    public override void Enter() {
        base.Enter();
        // player.Anim.SetBool(previousAnimBoolName, true);
        // if (countDown < 0f) {
        //     countDown = 1f / weapon.FireRate;
        //     weapon.ShootBullet();
        // }
        isAbilityDone = true;
    }

    // public void SetWeapon(Weapon weapon) {
    //     this.weapon = weapon;
    //     weapon.InitializeWeapon(this);
    // }

    public override void LogicUpdate() {
        base.LogicUpdate();
        // player.Anim.SetBool(previousAnimBoolName, false);
        // if (countDown > 0f) countDown -= Time.deltaTime;

    }

}
