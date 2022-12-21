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
    private List<CanPassiveSkillInfo> _passiveSkillInfoList = new List<CanPassiveSkillInfo>(); // �нú� ��ų 

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
    
    [ContextMenu("�нú� �׽�Ʈ ")]
    public void PassiveTest()
    {
        EventManager.Instance.TriggerEvent(EventsType.CheckPassiveSkill);
    }
    [ContextMenu("��Ƽ�� �׽�Ʈ")]
    public void ActiveTest()
    {
        EventManager.Instance.TriggerEvent(EventsType.CheckActiveSkill);
    }

    /// <summary>
    /// �нú� ��ų Ȯ�� 
    /// </summary>
    public void CheckPassiveSkill()
    {
        CheckRemoveSkill();

        // Ȱ��ȭ�� �нú� ��ų �ִ��� Ȯ�� 
        for (int i = 0; i < _skillInventorySO.skillList.Count; i++)
        {
            CanPassiveSkillInfo skillInfo = GetPassiveSkillInfo(_skillInventorySO.skillList[i].attackType);
            
            if(skillInfo != null && skillInfo.isCan == false) // ��ų�κ��丮�� �����鼭 Ȱ��ȭ �Ǿ� ���� �ʴٸ� 
            {
                ActivePassiveSkill(skillInfo.attackType);
                Debug.Log(skillInfo.attackType + "@@ �нú� Ȱ��ȭ @@");

            }
        }
    }

    /// <summary>
    /// �нú� ��ų Ȱ��ȭ 
    /// </summary>
    /// <param name="attackType"></param>
    public void ActivePassiveSkill(AttackType attackType)
    {
        CanPassiveSkillInfo skillInfo = GetPassiveSkillInfo(attackType);
        skillInfo.isCan = true;
        skillInfo.skillData.Enter();
    }

    /// <summary>
    /// �нú� ��ų ��Ȱ��ȭ 
    /// </summary>
    /// <param name="attackType"></param>
    public void InActivePassiveSkill(AttackType attackType)
    {
        CanPassiveSkillInfo skillInfo = GetPassiveSkillInfo(attackType);
        skillInfo.isCan = false;
        skillInfo.skillData.Exit();
    }

    /// <summary>
    /// �нú� ��ų ����Ʈ�� �ִ��� Ȯ�� 
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
    /// ������ �нú� ��ų �ִ��� Ȯ��
    /// </summary>
    private void CheckRemoveSkill()
    {
        foreach(var skillInfo in _passiveSkillInfoList)
        {
            bool isRemove = true; // �����ؾ� �ϳ� 
            if(skillInfo.isCan == true) // Ȱ��ȭ ������ ��ų 
            {
                for (int i = 0; i < _skillInventorySO.skillList.Count; i++) // ��ų �κ��丮�� �ִ��� Ȯ�� 
                {
                    CanPassiveSkillInfo canPassive = GetPassiveSkillInfo(_skillInventorySO.skillList[i].attackType);
                    if(canPassive != null)
                    {
                        isRemove = false;
                        break; 
                    }
                }

                if (isRemove == true) // ����? -> ���� 
                {
                    Debug.Log(skillInfo.attackType + "@@ �нú� ��Ȱ��ȭ @@");
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
