using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState {

    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;

    protected bool isAnimationFinished;
    protected bool isExitingState;

    protected float startTime;
    public float StartTime { get => startTime; }

    private int animBoolName;
    // private Weapon weapon;
    // private float countDown;

    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        this.animBoolName = Animator.StringToHash(animBoolName);
    }
    public virtual void Enter() {
        DoChecks();
        player.Anim.SetBool(animBoolName, true);
        startTime = Time.time;
        isAnimationFinished = false;
        isExitingState = false;
    }
    public virtual void Exit() {
        player.Anim.SetBool(animBoolName, false);
        isExitingState = true;
    }

    public virtual void LogicUpdate() {


    }
    public virtual void PhysicsUpdate() {
        DoChecks();
    }
    public virtual void DoChecks() { }
    public virtual void AnimationTrigger() { }
    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;

}
