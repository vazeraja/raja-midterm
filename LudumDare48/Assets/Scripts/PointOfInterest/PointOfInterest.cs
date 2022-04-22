using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PointOfInterest : MonoBehaviour {

    public static event Action<PointOfInterest> OnPoiEntered;

    [SerializeField] private string _poiName;

    public string PoiName { get => _poiName; }

    private string uniqueID = Guid.NewGuid().ToString("N");

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<ICollector>()?.OnCollect(Item.DEFAULT) == null) {
            return;
        } else {
            if (OnPoiEntered != null) {
                OnPoiEntered(this);
            }
        }
    }

}