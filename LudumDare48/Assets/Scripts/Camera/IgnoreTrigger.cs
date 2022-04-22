using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreTrigger : MonoBehaviour, IIgnoreObject {
    public bool IgnoreMe() {
        return true;
    }
}
