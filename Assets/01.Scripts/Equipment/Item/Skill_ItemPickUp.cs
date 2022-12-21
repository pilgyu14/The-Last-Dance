using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_ItemPickUp : PoolableMono
{
    private bool isTouch = false;
    public ItemInformationSO itemSO;

    private void Update()
    {
        if (isTouch == true)
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
        foreach (SkillInfo item in ItemUI.Instance.skillInventorySO.skillList)
        {
            if (item.skillName == itemSO.item.name)
            {
                if (item.skillLevel < ItemUI.Instance.inventorySO.maxItemValue)
                {
                    item.skillLevel++;
                    ItemUI.Instance.pickup.SetActive(false);
                    PoolManager.Instance.Push(this);
                    ItemUI.Instance.UpdateItemUI();
                    return;
                }
                return;
            }
        }
        if (ItemUI.Instance.skillInventorySO.skillList.Count < ItemUI.Instance.inventorySO.maxItemType)
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
