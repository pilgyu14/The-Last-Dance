using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/Information")]
public class ItemInformationSO : ScriptableObject
{
    public Item item;
    
    public void Copy(Item item)
    {
        this.item.itemPrefab = item.itemPrefab;
        this.item.itemImage = item.itemImage;
        this.item.name = item.name;
        this.item.value = item.value; 
    }
}
