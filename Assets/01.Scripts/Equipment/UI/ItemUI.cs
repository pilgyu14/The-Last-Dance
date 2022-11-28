using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ItemUI : MonoBehaviour
{
    [SerializeField]
    public InventorySO inventorySO;

    [SerializeField]
    public Image itemImage;

    private int pageIdx = 0;

    private void Start()
    {
        UpdateItemImage();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(pageIdx > 0)
            {
                pageIdx--;
                UpdateItemImage();
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if(pageIdx < inventorySO.maxItemType - 1)
            {
                pageIdx++;
                UpdateItemImage();
            }
        }
    }

    public void UpdateItemImage()
    {
        itemImage.sprite = inventorySO.itemList[pageIdx].itemImage;
    }

    public void InventoryClear()
    {
        foreach(Item item in inventorySO.itemList)
        {
            inventorySO.itemList.Remove(item);
        }
    }
}
