using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossController : Singleton<FinalBossController>, IDamageable {

    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private Vector2 hitForce = new Vector2(4f, 5f);
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float contactDamage = 15f;
    [SerializeField] private float playerCheckDistance = 5f;
    [SerializeField] private float drag;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float attackTime = 2f;
    [SerializeField] private List<Transform> nextPositions = new List<Transform>();

    private Rigidbody2D rb;
    private Animator anim;
    private float currentHealth;
    private bool isDetected;
    private bool bossFightStarted;
    private bool transformationDone;
    private bool isWalking;
    private bool hasGottenUp;
    private bool attackDone;
    private bool hasReachedNextPos;
    private int index;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        bossFightStarted = false;
        transformationDone = false;
        isWalking = false;
    }

#if UNITY_EDITOR
    private void OnGUI() {
        if (GUI.Button(new Rect(0, 0, 200, 50), "Start Fight")) {
            StartBossFight();
        }
    }

#endif
    private void Update() {
        if (hasGottenUp && isWalking && !bossFightStarted) {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        } else if (isWalking && transformationDone && bossFightStarted && !CheckPlayerInRange()) {
            rb.velocity = Player.Instance.transform.position.x > transform.position.x ? Vector2.right * speed : -Vector2.right * speed;
        }
    }

    private void FixedUpdate() {

        if (nextPositions[index].position.x > transform.position.x) {
            hasReachedNextPos = false;
        } else {
            hasReachedNextPos = true;
        }

        if (Player.Instance.transform.position.x > transform.position.x && hasReachedNextPos) {
            if (index + 1 < nextPositions.Count) {
                index++;
                hasReachedNextPos = false;
            }
        }


        if (Player.Instance.transform.position.x > transform.position.x && !bossFightStarted && !hasReachedNextPos) {
            anim.SetBool("idle", false);
            if (!isWalking) {
                anim.SetTrigger("getup");
                isWalking = true;
            }
        } else if (!bossFightStarted && hasReachedNextPos) {
            anim.SetBool("idle", true);
        } else if (transformationDone && bossFightStarted && CheckPlayerInRange()) {
            StartAttackPhase();
        }
    }


    public void MoveToNextLocation() {

    }


    public void StartBossFight() {
        bossFightStarted = true;
        anim.SetBool("idlewalk", false);
        isWalking = false;
        anim.SetTrigger("ultimateform");
    }

    public void StartAttackPhase() {
        anim.SetBool("bigwalk", false);
        // StartCoroutine(DoAttack());
        // Do attack, wait for animation done, then walk
    }
    public void OnAttackDone() {
        anim.SetBool("bigwalk", true);
        isWalking = true;
    }
    public void OnGetUpDone() {
        anim.SetBool("littlewalk", true);
        isWalking = true;
        hasGottenUp = true;
    }
    IEnumerator DoAttack() {

        isWalking = false;
        anim.SetTrigger("bigattack");
        var countDown = attackTime;
        while (countDown > 0f) {
            countDown -= Time.deltaTime;
            // do attack shoot something
            yield return default;
        }
        isWalking = true;
        anim.SetBool("bigwalk", true);
    }

    public void OnTransformationDone() {
        transformationDone = true;
        anim.SetBool("bigwalk", true);
    }

    public void TakeDamage(float damage, bool hitFromRight) {


        PlayDamageEffect();

        hitForce.x *= hitFromRight ? -1 : 1;

        rb.AddForce(hitForce, ForceMode2D.Impulse);

        currentHealth -= (int)damage;

        CinemachineShake.Instance.ShakeCamera(3f, 0.2f);

        if (currentHealth <= 0) {
            RandomDrop.SpawnRandomDrop(transform.position, Quaternion.identity);
            Destroy(this.gameObject, 0.1f);
        }
        Debug.Log("Creedit God" + " " + currentHealth);

        // Debug.Log(EnemyDelegateCount);
    }

    private void PlayDamageEffect() {
        var damageEffect = ObjectPooler.Instance.SpawnFromPool("damageEffect", transform.position, Quaternion.identity);
        var scale = transform.localScale;
        scale.x *= -1;
        damageEffect.transform.localScale = scale;
        damageEffect.transform.position = transform.position;

        damageEffect.GetComponent<ParticleSystem>().Play();
    }


    private void OnCollisionEnter2D(Collision2D other) {

    }

    private void OnTriggerEnter2D(Collider2D other) {

    }

    bool CheckPlayerInRange() {

        return Physics2D.Raycast(playerCheck.position, Vector2.right, playerCheckDistance, whatIsPlayer);
    }

    public void TakeDamage(float damage) {
        // Debug.Log("take damage calleed");
        // TakeDamage(damage, false);
        // throw new System.NotImplementedException();
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(playerCheck.position, playerCheck.position + (Vector3)(Vector2.right * playerCheckDistance));

    }


}
