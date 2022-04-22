using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreeditMart : MonoBehaviour {

    private void OnTriggerStay2D(Collider2D other) {
        Debug.Log("player entered");
        if (other.GetComponent<IIgnoreObject>()?.IgnoreMe() != null) return;
        var player = other.gameObject.GetComponent<IDamageable>();

        if (player != null) {
            player.TakeDamage(0);
        }
    }

}
