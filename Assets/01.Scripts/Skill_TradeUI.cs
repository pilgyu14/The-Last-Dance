using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Skill_TradeUI : MonoSingleton<Skill_TradeUI>
{
    public SkillInventorySO skillInventorySO;
    
    [SerializeField]
    private Image dropSkillImage;
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private TextMeshProUGUI descriptionText;
    
    [SerializeField]
    private List<Image> haveSkillImage = new List<Image>();
    [SerializeField]
    private List<bool> isSkillList = new List<bool>();

    private SkillInfo newSkill = new SkillInfo();

    private SkillInfo _curSelcetSkillINfo; 
    public void UpdateUI(SkillInfo skill)
    {
        newSkill.Copy(skill);

        dropSkillImage.sprite = skill.icon;
        nameText.text = skill.skillName;
        descriptionText.text = skill.skillDescription;

        for(int i = 0; i < skillInventorySO.skillList.Count; i++)
        {
            haveSkillImage[i].sprite = skillInventorySO.skillList[i].icon;
        }
    }

    public void OnDropButton()
    {
        gameObject.SetActive(false);
    }

    public void OnSKillToggle(int idx)
    {
        for(int i = 0; i < isSkillList.Count; i++)
        {
            isSkillList[i] = false;
        }
        isSkillList[idx] = true;
    }

    public void OnTradeButton()
    {
        bool isActive = false;
        for(int i = 0; i < isSkillList.Count; i++)
        {
            if(isSkillList[i] == true)
            {
                skillInventorySO.skillList[i].Copy(newSkill);
                isActive = true;
            }
        }
        if (isActive == false) return;
        ItemUI.Instance.UpdateSkillUI();
        gameObject.SetActive(false);
    }

}
