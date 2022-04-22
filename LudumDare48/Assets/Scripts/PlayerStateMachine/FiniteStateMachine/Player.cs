using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>, IDamageable, ICollector {


    #region State Variables 
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerDamagedState DamagedState { get; private set; }

    [SerializeField] private PlayerData playerData;
    [SerializeField] private Weapon weapon;

    #endregion

    #region Components
    public PlayerInputHandler InputHandler { get; private set; }
    public Animator Anim { get; private set; }
    public Rigidbody2D RB { get; private set; }
    public SpriteRenderer SR { get; private set; }

    #endregion

    #region Check Transforms
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ledgeCheck;

    #endregion

    #region Main Player Variables
    [SerializeField] private float maxHealth;
    [SerializeField] private float maxOxygen;
    public Vector2 CurrentVelocity { get; private set; }
    public int FacingDirection { get; private set; }
    public float MaxHP { get => maxHealth; }
    public float MaxOxygen { get => maxOxygen; }
    public float CurrentHP {
        get => LocalSave.Instance.saveData.health;
        set {
            LocalSave.Instance.saveData.health = value;
            UIManager.Instance.SetHPHUD();
        }
    }
    public float CurrentOxygen {
        get => LocalSave.Instance.saveData.oxygen;
        set {
            LocalSave.Instance.saveData.oxygen = value;
            UIManager.Instance.SetOxgyenHUD();
        }
    }
    public int CurrentGems {
        get => LocalSave.Instance.saveData.gems;
        set {
            LocalSave.Instance.saveData.gems = value;
            UIManager.Instance.SetGemsHUD();
        }
    }
    public int PlayerGunLevel {
        get => LocalSave.Instance.saveData.gunLevel;
        set {
            LocalSave.Instance.saveData.gunLevel = value;

        }
    }

    #endregion

    #region Other Variables
    private Vector2 previousVelocity;
    private float oxygenUsageRate = 5f;
    private GameObject damageEffect;
    private float countDown;
    private float oxygenCountDown;

    #endregion

    #region Unity Callback Functions
    private void Awake() {
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        JumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir");
        InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
        LandState = new PlayerLandState(this, StateMachine, playerData, "land");
        DamagedState = new PlayerDamagedState(this, StateMachine, playerData, "damaged");

        damageEffect = gameObject.FindInChildren("damageEffect");
    }
    private void Start() {
        Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();

        FacingDirection = 1;

        // CurrentHP = maxHealth = 100f;
        // CurrentOxygen = maxOxygen = 100f;

        StateMachine.Initialize(IdleState);
    }
    private void Update() {

        ConsumeOxygen();

        CurrentVelocity = RB.velocity;
        Shoot();
        StateMachine.CurrentState.LogicUpdate();

    }
    private void FixedUpdate() {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    #endregion

    #region Animation Triggers
    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();
    private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    #endregion

    public bool CheckIfGrounded() => Physics2D.OverlapCircle(groundCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);
    public bool CheckIfTouchingWall() => Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    public bool CheckIfTouchingLedge() => Physics2D.Raycast(ledgeCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);


    public void SetVelocityX(float velocity) {
        previousVelocity.Set(velocity, CurrentVelocity.y);
        RB.velocity = previousVelocity;
        CurrentVelocity = previousVelocity;
    }
    public void SetVelocityY(float velocity) {
        previousVelocity.Set(CurrentVelocity.x, velocity);
        RB.velocity = previousVelocity;
        CurrentVelocity = previousVelocity;
    }

    public void CheckIfShouldFlip(int xInput) {
        if (xInput != 0 && xInput != FacingDirection) {
            Flip();
        }
    }
    private void Flip() {
        FacingDirection *= -1;
        var scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void Shoot() {
        if (InputHandler.AttackInputs[(int)CombatInputs.PRIMARY]) {
            if (countDown <= 0) {
                countDown = 1f / weapon.FireRate;
                if (CurrentOxygen > 0f) {
                    weapon.ShootBullet();
                    CurrentOxygen--;
                }
            }
        }

        if (countDown >= 0) countDown -= Time.deltaTime;
    }

    public void TakeDamage(float damage) {

        StateMachine.ChangeState(DamagedState);

        CurrentHP -= (int)damage;

        PlayDamageEffect();

        if (CurrentHP <= 0) { gameObject.SetActive(false); }
    }
    private void ConsumeOxygen() {

        if (oxygenCountDown <= 0) {
            oxygenCountDown = oxygenUsageRate;
            if (CurrentOxygen > 0f) {
                CurrentOxygen--;
            } else {
                CurrentOxygen--;
            }
        }

        if (oxygenCountDown >= 0) oxygenCountDown -= Time.deltaTime;
    }

    private void PlayDamageEffect() {
        CinemachineShake.Instance.ShakeCamera(3f, 0.2f);

        var scale = transform.localScale;
        scale.x *= -1;
        damageEffect.transform.localScale = scale;

        damageEffect.GetComponent<ParticleSystem>().Play();
    }

    public bool OnCollect(Item item) {
        switch (item) {
            case Item.GEM: CurrentGems++; break;
            case Item.HEALTH: CurrentHP += 5; break;
            case Item.OXYGEN: CurrentOxygen += 5; break;
            default: break;

        }
        return true;
    }

    public void TakeDamage(float damage, bool hitFromRight) {
        TakeDamage(damage);
    }
}
