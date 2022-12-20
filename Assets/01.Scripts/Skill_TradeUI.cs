using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Skill_TradeUI : MonoSingleton<Skill_TradeUI>
{
    public SkillInventorySO skillInventorySO;

    public Image dropSkillImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    public List<Image> haveSkillImage = new List<Image>();

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

    public void OnSkill1Button()
    {
        skillInventorySO.skillList[0].Copy(newSkill);
        ItemUI.Instance.UpdateSkillUI();
        gameObject.SetActive(false);
    }

    public void OnSkill2Button()
    {
        skillInventorySO.skillList[1].Copy(newSkill);
        ItemUI.Instance.UpdateSkillUI();
        gameObject.SetActive(false);
    }

    public void OnSkill3Button()
    {
        skillInventorySO.skillList[2].Copy(newSkill);
        ItemUI.Instance.UpdateSkillUI();
        gameObject.SetActive(false);
    }

    public void OnSkill4Button()
    {
        skillInventorySO.skillList[3].Copy(newSkill);
        ItemUI.Instance.UpdateSkillUI();
        gameObject.SetActive(false);
    }

}
