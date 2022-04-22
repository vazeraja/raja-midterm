using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationToStateMachine : MonoBehaviour {
    public AttackState attackState;
    public DamagedState damagedState;

    private void Start() {
        damagedState = GetComponentInParent<Enemy1>().damagedState;
    }
    private void TriggerAttack() => attackState.TriggerAttack();
    private void FinishAttack() => attackState.FinishAttack();

    // private void TriggerDamaged() => GetComponentInParent<Enemy1>().damagedState.TriggerDamaged();
    public void FinishDamagedAnimation() {
        return;
        // var x = GetComponentInParent<Enemy1>().damagedState;
        // Debug.Log("hi");
        // Debug.Log(x);
        // x.FinishDamaged();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.GetComponent<IIgnoreObject>()?.IgnoreMe() != null) return;
        var entity = GetComponentInParent<Enemy1>();
        other.gameObject.GetComponent<IDamageable>()?.TakeDamage(entity.entityData.contactDamage);
        Player.Instance.DamagedState.HitSide(transform.position.x > Player.Instance.transform.position.x);
        Player.Instance.DamagedState.SetHitForce(4, 5);
        entity.StateMachine.ChangeState(entity.lookForPlayerState);
    }

}


