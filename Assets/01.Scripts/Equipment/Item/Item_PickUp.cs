using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_PickUp : PoolableMono
{
    private bool isTouch = false;
    public ItemInformationSO itemSO;

    private void Update()
    {
        if(isTouch == true)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                ItemAdd();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isTouch = true;
            ItemUI.Instance.pickupNameText.text = itemSO.item.name;
            ItemUI.Instance.pickup.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isTouch = false;
            ItemUI.Instance.pickup.SetActive(false);
        }
    }

    private void ItemAdd()
    {
        foreach (Item item in ItemUI.Instance.inventorySO.itemList)
        {
            if (item.name == itemSO.item.name)
            {
                if (item.value < ItemUI.Instance.inventorySO.maxItemValue)
                {
                    item.value++;
                    ItemUI.Instance.pickup.SetActive(false);
                    PoolManager.Instance.Push(this);
                    ItemUI.Instance.UpdateItemUI();
                    return;
                }
                return;
            }
        }
        if (ItemUI.Instance.inventorySO.itemList.Count < ItemUI.Instance.inventorySO.maxItemType)
        {
            Item item = new Item();
            item.Copy(itemSO.item); 
            ItemUI.Instance.inventorySO.itemList.Add(item);

            ItemUI.Instance.pickup.SetActive(false);
            PoolManager.Instance.Push(this);
            ItemUI.Instance.UpdateItemUI();
        }
    }

    public override void Reset()
    {
        itemSO.item.value = 1;
    }
}
