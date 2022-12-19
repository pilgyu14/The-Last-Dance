using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[Serializable]
public class Item
{
    public GameObject itemPrefab;
    public Sprite itemImage;
    public string name;
    public int value = 0;

    public void Copy(Item item)
    {
        this.itemPrefab = item.itemPrefab;
        this.itemImage = item.itemImage;
        this.name = item.name;
        this.value = item.value;
    }
}
