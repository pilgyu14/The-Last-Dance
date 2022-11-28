using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/InventorySO")]
public class InventorySO : ScriptableObject
{
    public int maxItemType;
    public int maxItemValue;
    public List<Item> itemList;
}
