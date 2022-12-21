using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanPassiveSkillInfo
{
    public bool isCan;
    public AttackType attackType;
    public SkillData<PlayerController> skillData;

    public CanPassiveSkillInfo(bool isCan, AttackType attackType, SkillData<PlayerController> skillData)
    {
        this.isCan = isCan;
        this.attackType = attackType;
        this.skillData = skillData;
    }
}


public class PassiveSkillManageComponent : MonoBehaviour
{
    [SerializeField]
    private SkillInventorySO _skillInventorySO;
    private PlayerController _owner;
    [SerializeField]
    private List<CanPassiveSkillInfo> _passiveSkillInfoList = new List<CanPassiveSkillInfo>(); // 패시브 스킬 

    private void Awake()
    {
        _owner = GetComponentInParent<PlayerController>();
    }
    private void Start()
    {
        _passiveSkillInfoList.Add(new CanPassiveSkillInfo(false, AttackType.CanThreeAttack, new ThreeAttackSkill(_owner)));
        _passiveSkillInfoList.Add(new CanPassiveSkillInfo(false, AttackType.DefaultAttackUpgrade, new DefaultAttackUpgradeSkill(_owner)));

        EventManager.Instance.StartListening(EventsType.CheckPassiveSkill, CheckActivePassiveSkill); 
    }
    private void Update()
    {
        UpdatePassiveSkill();
    }
    
    [ContextMenu("패시브 테스트 ")]
    public void PassiveTest()
    {
        EventManager.Instance.TriggerEvent(EventsType.CheckPassiveSkill);
    }
    [ContextMenu("액티브 테스트")]
    public void ActiveTest()
    {
        EventManager.Instance.TriggerEvent(EventsType.CheckActiveSkill);
    }

    public void CheckActivePassiveSkill()
    {
        for (int i = 0; i < _skillInventorySO.skillList.Count; i++)
        {
            CanPassiveSkillInfo skillInfo = GetPassiveSkillInfo(_skillInventorySO.skillList[i].attackType);
            
            if (skillInfo == null) continue; // 패시브 스킬이 저장되어 있지 않으면 리턴
            if (skillInfo.isCan == true ) //이미 활성화 되어 있다면 
            {
                InActivePassiveSkill(skillInfo.attackType);
                Debug.Log(skillInfo.attackType  + "@@ 패시브 비활성화 @@");
            }else
            {
                ActivePassiveSkill(skillInfo.attackType);
                Debug.Log(skillInfo.attackType + "@@ 패시브 활성화 @@");

            }
        }
    }

    /// <summary>
    /// 패시브 스킬 활성화 
    /// </summary>
    /// <param name="attackType"></param>
    public void ActivePassiveSkill(AttackType attackType)
    {
        CanPassiveSkillInfo skillInfo = GetPassiveSkillInfo(attackType);
        skillInfo.isCan = true;
        skillInfo.skillData.Enter();
    }

    /// <summary>
    /// 패시브 스킬 비활성화 
    /// </summary>
    /// <param name="attackType"></param>
    public void InActivePassiveSkill(AttackType attackType)
    {
        CanPassiveSkillInfo skillInfo = GetPassiveSkillInfo(attackType);
        skillInfo.isCan = false;
        skillInfo.skillData.Exit();
    }

    private CanPassiveSkillInfo GetPassiveSkillInfo(AttackType attackType)
    {
        foreach (var skillInfo in _passiveSkillInfoList)
        {
            if (skillInfo.attackType == attackType)
            {
                return skillInfo;
            }
        }
        return null;
    }
    private void UpdatePassiveSkill()
    {
        int count = _passiveSkillInfoList.Count;
        for (int i = 0; i < count; i++)
        {
            _passiveSkillInfoList[i].skillData.Stay();
        }
    }
}
