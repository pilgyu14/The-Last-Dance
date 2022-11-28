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
            if(pageIdx < inventorySO.maxItemType)
            {
                pageIdx++;
                UpdateItemImage();
            }
        }
    }

    public void UpdateItemImage()
    {
        itemImage = inventorySO.itemList[pageIdx].itemImage;
    }
}
