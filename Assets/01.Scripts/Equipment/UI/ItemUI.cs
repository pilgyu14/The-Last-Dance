using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ItemUI : MonoBehaviour
{
    [SerializeField]
    public InventorySO inventorySO;
    [SerializeField]
    public Image itemImage;
    [SerializeField]
    public GameObject pickup;
    [SerializeField]
    public TextMeshProUGUI pickupNameText;

    private int pageIdx = 0;

    private static ItemUI instance;
    public static ItemUI Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

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
            if(pageIdx < inventorySO.maxItemType - 1 && pageIdx < inventorySO.itemList.Count - 1)
            {
                pageIdx++;
                UpdateItemImage();
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
                    inventorySO.itemList.RemoveAt(pageIdx);
                    pageIdx = 0;
                }
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
