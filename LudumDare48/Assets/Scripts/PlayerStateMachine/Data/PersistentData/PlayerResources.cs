using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class PlayerResources {
    public float oxygen;
    public float health;
    public int gems; // trade in to pay off debt or purchase upgrade
    public int upgrades;
    public float debt;
    public int gunLevel;
    public HashSet<string> achievements = new HashSet<string>();

    public void Start() {
        // hashet first element gets yeeted for some reason
        LocalSave.Instance.saveData.achievements.Add("dummy");
    }
}
