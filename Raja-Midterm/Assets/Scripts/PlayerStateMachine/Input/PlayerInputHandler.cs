using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;


public enum CombatInputs {
    PRIMARY,
}
public class PlayerInputHandler : Singleton<PlayerInputHandler> {

    private PlayerInput playerInput;


    public Vector2 RawMovementInput { get; private set; }
    public int NormInputX { get; private set; }
    public int NormInputY { get; private set; }

    public bool JumpInput { get; private set; }
    public bool JumpInputStop { get; private set; }

    [SerializeField] private float inputHoldTime; // Fixes Double Jump from spamming Spacebar
    private float jumpinputStartTime;

    public bool[] AttackInputs { get; private set; }

    public static bool MenuInput { get; private set; }
    public GameObject startgametext;

    private void Start() {
        int count = Enum.GetValues(typeof(CombatInputs)).Length;
        AttackInputs = new bool[count];
        playerInput = GetComponent<PlayerInput>();
    }
    private void Update() {
        CheckJumpInputHoldTime();
    }

    public void OnMoveInput(InputAction.CallbackContext context) {
        RawMovementInput = context.ReadValue<Vector2>();

        NormInputX = (int)(RawMovementInput * Vector2.right).normalized.x;
        NormInputY = (int)(RawMovementInput * Vector2.up).normalized.y;
    }
    public void OnJumpInput(InputAction.CallbackContext context) {
        if (context.started) {
            JumpInput = true;
            JumpInputStop = false;
            jumpinputStartTime = Time.time;
        }
        if (context.canceled) {
            JumpInputStop = true;
        }
    }
    public void OnMenuInput(InputAction.CallbackContext context) {
        if (context.started && !MenuInput) {
            MenuInput = true;
            // startgametext.SetActive(false);
        } else if ((context.started && MenuInput)) {
            MenuInput = false;
        }
    }
    private void CheckJumpInputHoldTime() { if (Time.time >= jumpinputStartTime + inputHoldTime) JumpInput = false; }

    public void UseJumpInput() => JumpInput = false;
    public void OnPrimaryAttackInput(InputAction.CallbackContext context) {
        if (context.started) { AttackInputs[(int)CombatInputs.PRIMARY] = true; }
        if (context.canceled) { AttackInputs[(int)CombatInputs.PRIMARY] = false; }
    }

}
