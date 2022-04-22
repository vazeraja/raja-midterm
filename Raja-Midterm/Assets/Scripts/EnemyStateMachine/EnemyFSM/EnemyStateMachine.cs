using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine {

    public EnemyState CurrentState { get; private set; }
    public bool preventStateChange;

    /// <summary>
    /// Initializes starting state for entity
    /// </summary>
    /// <param name="startingState"></param>
    public void Initialize(EnemyState startingState) {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    /// <summary>
    /// Called whenever you want to change the state of the entity
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(EnemyState newState) {
        if (preventStateChange) return;
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

}
