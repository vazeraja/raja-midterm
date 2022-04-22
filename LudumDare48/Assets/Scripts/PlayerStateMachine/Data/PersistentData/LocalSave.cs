using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalSave : Singleton<LocalSave> {
    public PlayerResources saveData = new PlayerResources();

    private void Start() {
        this.saveData = GlobalSave.Instance.saveData;
    }
    private void Update() {
        // Save();
    }
    public void Save() {
        if (GlobalSave.Instance != null) GlobalSave.Instance.saveData = this.saveData;
    }
}
