using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossState {
    protected FinalBoss boss;
    protected BossStateMachine stateMachine;
    protected FinalBossData bossData;

    protected bool isAnimationFinished;
    protected bool isExitingState;

    protected float startTime;
    public float StartTime { get => startTime; }

    private int animBoolName;
    // private Weapon weapon;
    // private float countDown;

    public BossState(FinalBoss boss, BossStateMachine stateMachine, FinalBossData bossData, string animBoolName) {
        this.boss = boss;
        this.stateMachine = stateMachine;
        this.bossData = bossData;
        this.animBoolName = Animator.StringToHash(animBoolName);
    }
    public virtual void Enter() {
        DoChecks();
        boss.Anim.SetBool(animBoolName, true);
        startTime = Time.time;
        isAnimationFinished = false;
        isExitingState = false;
    }
    public virtual void Exit() {
        boss.Anim.SetBool(animBoolName, false);
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
