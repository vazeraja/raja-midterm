using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShroomController : MonoBehaviour, IDamageable {
    Rigidbody2D rb;

    [SerializeField] private float health = 10f;

    [SerializeField] float rotateSpeed = 10f;
    [SerializeField] float speed;

    [SerializeField] float contactDamage;

    [SerializeField] LayerMask whatIsPlayer;

    [SerializeField] Transform playerCheck;
    [SerializeField] float playerCheckDistance;

    [SerializeField] Vector2 hitForce;
    [SerializeField] float drag;

    bool isPlayerInRange;
    bool isDetected;

    float destroyTime = 3f;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        isPlayerInRange = CheckPlayerInRange();
        PlayerDetected();
    }

    void PlayerDetected() {
        if (isPlayerInRange) {
            isDetected = true;
            rb.velocity = speed * Vector2.left;
            transform.Rotate(0, 0, rotateSpeed);
            StartCoroutine(DestroyShroom());
        } else if (isDetected) {
            transform.Rotate(0, 0, rotateSpeed);
        }
    }

    bool CheckPlayerInRange() => Physics2D.Raycast(playerCheck.position, -Vector2.right, playerCheckDistance, whatIsPlayer);

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<IIgnoreObject>()?.IgnoreMe() != null) return;

        var player = other.gameObject.GetComponent<IDamageable>();

        if (player != null) {
            player.TakeDamage(contactDamage);
            Destroy(this.gameObject);
        }
    }
    public void TakeDamage(float damage) {
        CinemachineShake.Instance.ShakeCamera(3f, 0.2f);
        health -= (int)damage;
        if (health <= 0) {
            RandomDrop.SpawnRandomDrop(transform.position, Quaternion.identity);
            Destroy(this.gameObject, 0.1f);
        }
        rb.AddForce(hitForce, ForceMode2D.Impulse);
        Debug.Log(health);
    }
    IEnumerator DestroyShroom() {
        yield return new WaitForSeconds(destroyTime);
        Destroy(this.gameObject);
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(playerCheck.position, playerCheck.position + (Vector3)(-Vector2.right * playerCheckDistance));

    }

    public void TakeDamage(float damage, bool hitFromRight) {
        PlayDamageEffect();

        health -= (int)damage;

        hitForce.x *= hitFromRight ? -1 : 1;

        rb.drag = drag;

        rb.AddForce(hitForce, ForceMode2D.Impulse);

        if (health <= 0) {
            RandomDrop.SpawnRandomDrop(transform.position, Quaternion.identity);
            Destroy(this.gameObject, 0.1f);
        }
        Debug.Log(health);
    }
    private void PlayDamageEffect() {
        CinemachineShake.Instance.ShakeCamera(3f, 0.2f);
        var damageEffect = ObjectPooler.Instance.SpawnFromPool("damageEffect", transform.position, Quaternion.identity);
        var scale = transform.localScale;
        scale.x *= -1;
        damageEffect.transform.localScale = scale;
        damageEffect.transform.position = transform.position;

        damageEffect.GetComponent<ParticleSystem>().Play();
    }
}
