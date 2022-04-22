using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : MonoBehaviour, IDamageable {

#if UNITY_EDITOR

    private void OnGUI() {
        GUIStyle gs = new GUIStyle();
        gs.fontSize = 50;
        GUI.contentColor = Color.red;
        string state = StateMachine.CurrentState.ToString();

        GUI.Label(new Rect(0, 0, 1000, 100), state, gs);
    }

#endif



    #region State Variables
    public BossStateMachine StateMachine { get; private set; }

    public BossIdleState IdleState { get; private set; }
    public BossLittleWalkState LittleWalkState { get; private set; }
    public BossTransformationState TransformationState { get; private set; }
    public BossBigWalkState BigWalkState { get; private set; }
    public BossAttackState AttackState { get; private set; }

    [SerializeField] private FinalBossData bossData;

    // weapon

    #endregion


    #region Components

    public Animator Anim { get; private set; }
    public Rigidbody2D RB { get; private set; }
    public SpriteRenderer SR { get; private set; }

    #endregion

    #region Check Transforms

    [SerializeField] private Transform attackPosition;
    [SerializeField] private List<Transform> checkPoints = new List<Transform>();
    [SerializeField] private Transform playerCheck;
    private int index;

    public int CheckPtIndex {
        get => index;
        set {
            index = value < checkPoints.Count ? value : index;
        }
    }

    public List<Transform> CheckPoints { get => checkPoints; }
    public Vector3 AttackPosition { get => attackPosition.position; }
    public Vector3 PlayerCheck { get => playerCheck.position; }

    #endregion

    #region Main Boss Variables
    [SerializeField] private float maxHealth;
    public Vector2 CurrentVelocity { get; private set; }
    public int FacingDirection { get; private set; }
    public float MaxHP { get => maxHealth; }
    private float currentHealth;
    public bool IsInvincible { get; set; }

    #endregion


    private void Awake() {
        // create all states

        StateMachine = new BossStateMachine();

        IdleState = new BossIdleState(this, StateMachine, bossData, "idle");
        LittleWalkState = new BossLittleWalkState(this, StateMachine, bossData, "littleWalk");
        TransformationState = new BossTransformationState(this, StateMachine, bossData, "ultimateForm");
        BigWalkState = new BossBigWalkState(this, StateMachine, bossData, "bigWalk");
        AttackState = new BossAttackState(this, StateMachine, bossData, "bigAttack");
    }
    private void Start() {
        Anim = GetComponent<Animator>();
        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();

        FacingDirection = 1; // right
        currentHealth = maxHealth;
        IsInvincible = true;

        StateMachine.Initialize(IdleState);
    }

    private void Update() {
        CurrentVelocity = RB.velocity;
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate() {
        StateMachine.CurrentState.PhysicsUpdate();
    }


    // Helpers
    public void SetCurrentVelocity(Vector2 velocity) {
        CurrentVelocity = velocity;
        RB.velocity = velocity;
    }

    private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();


    public bool CheckIfPlayerInRadius() => Physics2D.OverlapCircle(PlayerCheck, bossData.playerDetectionRadius, bossData.whatIsPlayer);

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(PlayerCheck, 5f);
    }

    public void TakeDamage(float damage) {
        TakeDamage(damage, true);
    }

    public void TakeDamage(float damage, bool hitFromRight) {

        PlayDamageEffect();

        if (IsInvincible) return;

        currentHealth -= (int)damage;

        CinemachineShake.Instance.ShakeCamera(3f, 0.2f);

        if (currentHealth <= 0) {
            RandomDrop.SpawnRandomDrop(transform.position, Quaternion.identity);
            Destroy(this.gameObject, 0.1f);
        }
        Debug.Log("Creedit God " + currentHealth);
    }

    private void PlayDamageEffect() {
        var damageEffect = ObjectPooler.Instance.SpawnFromPool("damageEffect", transform.position, Quaternion.identity);
        var scale = transform.localScale;
        scale.x *= -1;
        damageEffect.transform.localScale = scale;
        damageEffect.transform.position = transform.position;

        damageEffect.GetComponent<ParticleSystem>().Play();
    }
}
