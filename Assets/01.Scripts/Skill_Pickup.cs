using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Pickup : PoolableMono
{
    private bool isTouch = false;
    public AttackSO attackSO;

    void Update()
    {
        if (isTouch == true)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                SkillAdd();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isTouch = true;
            ItemUI.Instance.pickupNameText.text = attackSO.skillInfo.skillName;
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

    private void SkillAdd()
    {
        foreach(SkillInfo info in ItemUI.Instance.skillInventorySO.skillList)
        {
            if(info.skillName == attackSO.skillInfo.skillName)
            {
                info.skillLevel++;
                ItemUI.Instance.pickup.SetActive(false);
                PoolManager.Instance.Push(this);
                ItemUI.Instance.UpdateSkillUI();
                return;
            }
        }
        SkillInfo skill = new SkillInfo();
        skill.Copy(attackSO.skillInfo);
        if (ItemUI.Instance.skillInventorySO.skillList.Count >= 4)
        {
            ItemUI.Instance.skillTrade.SetActive(true);
            Skill_TradeUI.Instance.UpdateUI(skill);
            ItemUI.Instance.pickup.SetActive(false);
            PoolManager.Instance.Push(this);
            return;
        }
        else
        {
            ItemUI.Instance.skillInventorySO.skillList.Add(skill);

            ItemUI.Instance.pickup.SetActive(false);
            PoolManager.Instance.Push(this);
            ItemUI.Instance.UpdateSkillUI();
        }
    }

    public override void Reset()
    {
        
    }
}
