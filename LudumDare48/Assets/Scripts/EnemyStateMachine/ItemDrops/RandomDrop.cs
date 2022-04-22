using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class RandomDrop {
    public static void SpawnRandomDrop(Vector3 position, Quaternion quaternion) {
        int names = Enum.GetNames(typeof(Item)).Length;
        Item item = (Item)UnityEngine.Random.Range(0, names);
        if (item.Equals(Item.DEFAULT)) return;
        ObjectPooler.Instance.SpawnFromPool(item.ToString(), position, quaternion);
    }
}
