using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/ItemSO")]
public class InventorySO : ScriptableObject
{
    public Dictionary<GameObject, int> itemDictionary;
}
