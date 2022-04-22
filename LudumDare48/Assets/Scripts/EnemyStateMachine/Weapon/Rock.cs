using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour, IPooledObject {
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector3 scale;

    public float Speed { private get; set; }
    public Vector2 Direction { private get; set; }
    public float Damage { private get; set; }
    public float DestroyDelay { private get; set; }


    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        scale = transform.localScale;

    }

    public void OnObjectSpawn() {
        // StartCoroutine(DestroyBullet());
    }

    public void Shoot() {
        sr.flipX = (Direction.x < 0);
        rb.velocity = Direction * Speed;
        transform.localScale = Vector3.one * 1.5f;
    }

    private void FixedUpdate() {
        Accelerate();
    }

    IEnumerator DestroyBullet() {
        yield return new WaitForSeconds(DestroyDelay);
        // Destroy(this.gameObject, 0.1f);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<IIgnoreObject>()?.IgnoreMe() != null) return;
        other.gameObject.GetComponent<IDamageable>()?.TakeDamage(Damage);
        Player.Instance.DamagedState.HitSide(transform.position.x > Player.Instance.transform.position.x);
        Player.Instance.DamagedState.SetHitForce(4, 5);
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D other) {

        gameObject.SetActive(false);
    }

    private void Accelerate() {
        rb.velocity *= 1.01f;
        // transform.localScale = scale;
    }
}
