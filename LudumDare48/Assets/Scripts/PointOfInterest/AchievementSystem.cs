using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementSystem : MonoBehaviour {

    public GameObject dialogueBox;

    private void Start() {
        PlayerPrefs.DeleteAll();
        PointOfInterest.OnPoiEntered += OnPoiEnteredNotification;
    }

    private void OnDestroy() {
        PointOfInterest.OnPoiEntered -= OnPoiEnteredNotification;
    }
    private void OnPoiEnteredNotification(PointOfInterest poi) {
        string achievementKey = "achievement-" + poi.PoiName;

        if (LocalSave.Instance.saveData.achievements.Contains(achievementKey)) {
            return;
        } else {
            LocalSave.Instance.saveData.achievements.Add(achievementKey);
            Debug.Log("hashset " + string.Join("", LocalSave.Instance.saveData.achievements));

            dialogueBox.SetActive(true);

            dialogueBox.GetComponentInChildren<SpriteLetterSystem>().GenerateBigText($"unlocked: <c=(255,50,120)><w>{poi.PoiName}</w></c>");
            StartCoroutine(RemoveDialoguePanel());
        }
    }
    IEnumerator RemoveDialoguePanel() {
        yield return new WaitForSeconds(2f);
        dialogueBox.SetActive(false);
    }
}
