using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class Enemy1 : Entity, IDamageable {

    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private string enemyName;
    [SerializeField] private Vector2 hitForce = new Vector2(10, 5);

    public string EnemyName { get { return enemyName; } }

    private event Action<Enemy1> enemyDelegate;
    public int EnemyDelegateCount { get { return enemyDelegate?.GetInvocationList().Length ?? 0; } }

    public E1_IdleState idleState { get; private set; }
    public E1_MoveState moveState { get; private set; }
    public E1_PlayerDetectedState playerDetectedState { get; private set; }
    public E1_ChargeState chargeState { get; private set; }
    public E1_LookForPlayerState lookForPlayerState { get; private set; }
    public E1_DamagedState damagedState { get; private set; }

    [SerializeField] private SO_IdleState idleStateData;
    [SerializeField] private SO_MoveState moveStateData;
    [SerializeField] private SO_PlayerDetected playerDetectedData;
    [SerializeField] private SO_ChargeState chargeStateData;
    [SerializeField] private SO_LookForPlayer lookForPlayerStateData;

    public MoleRockThrow rockThrow;


    public override void Start() {
        base.Start();

        currentHealth = maxHealth;

        moveState = new E1_MoveState(this, StateMachine, "move", moveStateData, this);
        idleState = new E1_IdleState(this, StateMachine, "idle", idleStateData, this);
        playerDetectedState = new E1_PlayerDetectedState(this, StateMachine, "playerDetected", playerDetectedData, this);
        chargeState = new E1_ChargeState(this, StateMachine, "charge", chargeStateData, this);
        lookForPlayerState = new E1_LookForPlayerState(this, StateMachine, "lookForPlayer", lookForPlayerStateData, this);
        damagedState = new E1_DamagedState(this, StateMachine, "damaged", this);

        playerDetectedState.SetWeapon(rockThrow);

        StateMachine.Initialize(idleState);
    }
    public void TakeDamage(float damage, bool hitFromRight) {


        PlayDamageEffect();
        damagedState.HitSide(hitFromRight);
        damagedState.SetHitForce(hitForce.x, hitForce.y);
        StateMachine.ChangeState(damagedState);

        currentHealth -= (int)damage;

        CinemachineShake.Instance.ShakeCamera(3f, 0.2f);

        if (currentHealth <= 0) {
            if (enemyDelegate != null) enemyDelegate(this);
            RandomDrop.SpawnRandomDrop(AliveGO.transform.position, Quaternion.identity);
            Destroy(this.gameObject, 0.1f);
        }
        Debug.Log(EnemyName + " " + currentHealth);

        // Debug.Log(EnemyDelegateCount);
    }

    private void PlayDamageEffect() {
        var damageEffect = ObjectPooler.Instance.SpawnFromPool("damageEffect", transform.position, Quaternion.identity);
        var scale = AliveGO.transform.localScale;
        scale.x *= -1;
        damageEffect.transform.localScale = scale;
        damageEffect.transform.position = AliveGO.transform.position;

        damageEffect.GetComponent<ParticleSystem>().Play();
    }

    public override void OnDrawGizmos() {
        base.OnDrawGizmos();

        Gizmos.color = Color.green;
    }

    public void AddDelegate(Action<Enemy1> func) { if (EnemyDelegateCount < 1) enemyDelegate += func; }

    public void RemoveDelegate(Action<Enemy1> func) { if (EnemyDelegateCount > 0) enemyDelegate -= func; }

    public void TakeDamage(float damage) {
        throw new NotImplementedException();
    }
}
