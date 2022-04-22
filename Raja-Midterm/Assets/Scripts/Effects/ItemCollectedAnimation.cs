using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollectedAnimation : MonoBehaviour {
    [SerializeField] private float animationLength = 1f;
    [SerializeField] private Item item;
    private SpriteRenderer sp;

    IEnumerator Collection() {
        var countDown = animationLength;
        var pos = transform.position;
        var color = sp.color;
        while (countDown >= 0) {
            countDown -= Time.deltaTime;

            pos.y += 0.01f;
            transform.position = pos;

            color.a = countDown;
            sp.color = color;

            yield return default;
        }
        // Destroy(this.gameObject);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<ICollector>()?.OnCollect(item) != null) {
            sp = GetComponent<SpriteRenderer>();
            StartCoroutine(Collection());
        }
    }
}
