using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ItemUI : MonoSingleton<ItemUI>
{
    [SerializeField]
    public InventorySO inventorySO;
    [SerializeField]
    public Image itemImage;
    [SerializeField]
    public TextMeshProUGUI itemNameText;
    [SerializeField]
    public TextMeshProUGUI itemCntText;

    [SerializeField]
    public GameObject pickup;
    [SerializeField]
    public TextMeshProUGUI pickupNameText;

    private int pageIdx = 0;

    private void Start()
    {
        UpdateItemUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(pageIdx > 0)
            {
                pageIdx--;
                UpdateItemUI();
            }
            else
            {
                pageIdx = inventorySO.itemList.Count - 1;
                UpdateItemUI();
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if(pageIdx < inventorySO.itemList.Count - 1)
            {
                pageIdx++;
                UpdateItemUI();
            }
            else
            {
                pageIdx = 0;
                UpdateItemUI();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if(inventorySO.itemList[pageIdx].value > 0)
            {
                inventorySO.itemList[pageIdx].value--;

                Item_Base item = PoolManager.Instance.Pop(inventorySO.itemList[pageIdx].itemPrefab.name) as Item_Base;
                Vector3 startPos = GameManager.Instance.PlayerTrm.position;
                startPos.y += 1f;
                Quaternion rot = GameManager.Instance.PlayerTrm.rotation;
                
                item.SetPosAndRot(startPos, rot);
                if(inventorySO.itemList[pageIdx].value <= 0)
                {
                    if(inventorySO.itemList.Count != 1)
                    {
                        inventorySO.itemList.RemoveAt(pageIdx);
                    }
                    pageIdx = 0;
                }
                UpdateItemUI();
            }
        }
    }

    public void UpdateItemUI()
    {
        itemImage.sprite = inventorySO.itemList[pageIdx].itemImage;
        itemCntText.text = inventorySO.itemList[pageIdx].value.ToString();
        itemNameText.text = inventorySO.itemList[pageIdx].name;
    }

    public void InventoryClear()
    {
        foreach(Item item in inventorySO.itemList)
        {
            inventorySO.itemList.Remove(item);
        }
    }
}
