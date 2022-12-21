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

    private ThreeAttackSkill threeAttackSkill;
    private DefaultAttackUpgradeSkill defaultAttackUpgradeSkill;

    private void Awake()
    {
        _owner = GetComponentInParent<PlayerController>();

        threeAttackSkill = GetComponent<ThreeAttackSkill>();
        defaultAttackUpgradeSkill = GetComponent<DefaultAttackUpgradeSkill>(); 
    }
    private void Start()
    {
        threeAttackSkill.Init(_owner);
        defaultAttackUpgradeSkill.Init(_owner);

        _passiveSkillInfoList.Add(new CanPassiveSkillInfo(false, AttackType.CanThreeAttack, threeAttackSkill));
        _passiveSkillInfoList.Add(new CanPassiveSkillInfo(false, AttackType.DefaultAttackUpgrade, defaultAttackUpgradeSkill));

        EventManager.Instance.StartListening(EventsType.CheckPassiveSkill, CheckPassiveSkill); 
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

    /// <summary>
    /// 패시브 스킬 확인 
    /// </summary>
    public void CheckPassiveSkill()
    {
        CheckRemoveSkill();

        // 활성화할 패시브 스킬 있는지 확인 
        for (int i = 0; i < _skillInventorySO.skillList.Count; i++)
        {
            CanPassiveSkillInfo skillInfo = GetPassiveSkillInfo(_skillInventorySO.skillList[i].attackType);
            
            if(skillInfo != null && skillInfo.isCan == false) // 스킬인벤토리에 있으면서 활성화 되어 있지 않다면 
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

    /// <summary>
    /// 패시브 스킬 리스트에 있는지 확인 
    /// </summary>
    /// <param name="attackType"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 삭제할 패시브 스킬 있는지 확인
    /// </summary>
    private void CheckRemoveSkill()
    {
        foreach(var skillInfo in _passiveSkillInfoList)
        {
            bool isRemove = true; // 삭제해야 하나 
            if(skillInfo.isCan == true) // 활성화 상태인 스킬 
            {
                for (int i = 0; i < _skillInventorySO.skillList.Count; i++) // 스킬 인벤토리에 있는지 확인 
                {
                    CanPassiveSkillInfo canPassive = GetPassiveSkillInfo(_skillInventorySO.skillList[i].attackType);
                    if(canPassive != null)
                    {
                        isRemove = false;
                        break; 
                    }
                }

                if (isRemove == true) // 없다? -> 삭제 
                {
                    Debug.Log(skillInfo.attackType + "@@ 패시브 비활성화 @@");
                    InActivePassiveSkill(skillInfo.attackType);
                }
            }
        }
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
