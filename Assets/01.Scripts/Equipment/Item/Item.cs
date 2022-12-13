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
}
