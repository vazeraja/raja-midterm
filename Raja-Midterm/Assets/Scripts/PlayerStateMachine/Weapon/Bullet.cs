using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IPooledObject {
    private Animator animator;
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
        animator = GetComponent<Animator>();
        scale = transform.localScale;
    }

    public void OnObjectSpawn() {
        // StartCoroutine(DestroyBullet());
        // animator.Play("Fire");
    }

    public void Shoot() {
        sr.flipX = (Direction.x < 0);
        rb.velocity = Direction * (Speed + Mathf.Abs(Player.Instance.CurrentVelocity.x));
        // transform.localScale = Vector3.one;
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
        if (other.GetComponent<IIgnoreObject>()?.IgnoreMe() != null) return;
        other.GetComponentInParent<IDamageable>()?.TakeDamage(Damage, transform.position.x > other.transform.position.x);
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        gameObject.SetActive(false);
    }

    private void Accelerate() {
        rb.velocity *= 1.01f;
        // scale.x = transform.localScale.x * 1.01f;
        // scale.y = transform.localScale.y * 1.01f;
        // transform.localScale = scale;
    }
}


