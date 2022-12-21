using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ItemDropper : MonoBehaviour
{
    [SerializeField]
    private float dropPower = 2f;

    [Header("Å×ÀÌºí")]
    [SerializeField]
    private ItemTableSO itemTableSO;
    [SerializeField]
    private SkillTableSO skillTableSO;

    [Header("È®·ü")]
    [SerializeField]
    [Range(0, 1f)]
    private float itemDropChance, skillDropChance;

    public void DropItemAndSkill()
    {
        DropItem();
        DropSkill(); 
    }
    public void DropItem()
    {
        float dropVar = Random.value;
        if(dropVar < itemDropChance)
        {
            int randomIdx = Random.Range(0, itemTableSO.itemTable.Count);
            PoolableMono item = PoolManager.Instance.Pop(itemTableSO.itemTable[randomIdx].item.itemPrefab.name);
            item.transform.position = transform.position;

            Vector3 offset = Vector3.up * 2f;

            item.transform.DOJump(transform.position + offset, dropPower, 1, 0.3f);
        }
    }

    public void DropSkill()
    {
        float dropVar = Random.value;
        if (dropVar < skillDropChance)
        {
            int randomIdx = Random.Range(0, skillTableSO.skillTable.Count);
            PoolableMono item = PoolManager.Instance.Pop("Skill_Pickup" + skillTableSO.skillTable[randomIdx].skillInfo.skillName);
            item.transform.position = transform.position;

            Vector3 offset = Random.insideUnitCircle;

            item.transform.DOJump(transform.position + offset, dropPower, 1, 0.3f);
        }
    }
}
