using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSave : MonoBehaviour {
    public static GlobalSave Instance;
    public PlayerResources saveData = new PlayerResources();
    private void Awake() {
        if (Instance == null) {
            DontDestroyOnLoad(gameObject);
            Instance = this;
            saveData.Start();
        } else if (Instance != this) {
            Destroy(gameObject);
        }
    }
}
