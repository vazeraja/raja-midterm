using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICreeditMart : MonoBehaviour {

    private TextMeshProUGUI debtMeter;
    private TextMeshProUGUI gunLevel;
    private TextMeshProUGUI o2Meter;

    private SpriteLetterSystem creeditDialoguePanel;

    private float totalTime = 3f;
    private float timeRemaining = 3f;
    private bool textTrigger;
    private int dialogueLength;
    private int dialogueCount = 0;
    private string[] creeditDialogue = {
        "My name is <c=(65,105,225)>Creedit</c> and I'm here to tell you that you are in deep shit",
        "Because you have not yet paid me my due, I have trapped you in this cave ...",
        "You have <c=(34,139,34)>$5000</c> in lost debt payments to owe me ...",
        "Collect me at least 10 of the <c=(128,0,128)>purple gems</c> and we can settle this ...",
        "Good luck <c=(0,100,0)>Deeto</c> ... ",
        "Or else ...",
        "Press M to start the game",
    };



    private void Awake() {
        debtMeter = gameObject.FindInChildren("UI_CURRENT_DEBT_AMOUNT").GetComponent<TextMeshProUGUI>();
        gunLevel = gameObject.FindInChildren("UI_CURRENT_GUN_LEVEL").GetComponent<TextMeshProUGUI>();
        o2Meter = gameObject.FindInChildren("UI_CURRENT_O2_AMOUNT").GetComponent<TextMeshProUGUI>();

        creeditDialoguePanel = gameObject.FindInChildren("Dialogue_Panel_Creedit").GetComponentInChildren<SpriteLetterSystem>();

    }
    private void Start() {
        LocalSave.Instance.saveData.debt = 5000f;

        dialogueLength = creeditDialogue.Length;
        textTrigger = true;
    }
    private void Update() {
        debtMeter.text = LocalSave.Instance.saveData.debt.ToString();
        gunLevel.text = LocalSave.Instance.saveData.gunLevel.ToString();
        o2Meter.text = LocalSave.Instance.saveData.oxygen.ToString();

        if (dialogueCount > dialogueLength) {
            textTrigger = false;
        }

        if (textTrigger) {
            if (timeRemaining > 0) {
                timeRemaining -= Time.deltaTime;
            } else {
                StartMenuDialogue();
                timeRemaining = totalTime;
            }
        }
    }
    void StartMenuDialogue() {
        if (dialogueCount < dialogueLength) {
            creeditDialoguePanel.GenerateSmallText(creeditDialogue[dialogueCount]);
            dialogueCount++;
        } else {
            return;
        }
    }
}
