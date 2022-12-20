using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSaveComponent
{
    private SkillInventorySO _skillInventorySO;
    private InputModule _inputModule;
    private SkillComponent _skillComponent; 

    public void Init(InputModule inputModule,SkillComponent skillComponent, SkillInventorySO skillInventorySO)
    {
        this._inputModule = inputModule;
        this._skillComponent = skillComponent;
        this._skillInventorySO = skillInventorySO;

        //    EventManager.Instance.StartListening(EventsType.SetActiveSkillInput, (x) => SaveInputKey((KeyCode)x)); 
        EventManager.Instance.StartListening(EventsType.CheckActiveSkill, CheckActiveSkill);
    }

    /// <summary>
    /// ��Ƽ�� ��ų ��� �� ���� 
    /// </summary>
    public void CheckActiveSkill()
    {
        // Ű ��ϵǾ� �ִ� �͵� Ȯ���ϰ�
        // Ű�� ��ϵǾ� �ִ� attackBinds�� �� 
        // ��ų �κ��丮�� �ִ��� Ȯ���ؼ� ������ 
        // Ű ��ϵǾ� �ִ� �� ����  

        for(int i =0;i < _inputModule.skillInputList.Count; i++)
        {
            InputBinding skillInput = _inputModule.skillInputList[i]; 
            bool isRemove = true;
            for (int j = 0; j < _skillInventorySO.skillList.Count; j++) // ��ų����Ʈ�� �ִ� ����Ÿ���� 
            {
                AttackData attackData = _skillComponent.GetAttackData(_skillInventorySO.skillList[(int)skillInput.keyCode - 49].attackType); // ��ų ������ �����Ծ�

                if (attackData != null && attackData.callback == skillInput.callback) // �Է¿� ��ϵǾ� �����鼭 ��ų�κ��丮���� ������  ���� ���� 
                {
                    isRemove = false;
                }
            }
            if (isRemove == true) // �����ٸ� ���� 
            {
                RemoveInputKey(skillInput.keyCode);
            }
        }


        // ��ų ����Ʈ�� �ִµ�
        // Ű ��� �ȵǾ� ������ 
        // ����Ʈ �ε��� + 49 �� Ű�ڵ� �����ͼ� ��� 
        for (int i = 0; i < _skillInventorySO.skillList.Count; i++) // ��ų����Ʈ�� �ִ� ����Ÿ���� 
        {
            AttackData attackData = _skillComponent.GetAttackData(_skillInventorySO.skillList[i].attackType); // ���� ���� ��ų�� ��Ƽ�� ��ų�� �߿� �ִ��� Ȯ�� 

            KeyCode keyCode = (KeyCode)(i + 49);
            if (attackData != null && _inputModule.IsKeyRegister(keyCode) == false) // ��ų����Ʈ�� �����鼭 Ű ����� �ȵǾ� �ִٸ� ��� �ϱ� 
            {
                SaveInputKey(keyCode);
            }
        }
    }

    // � Ű ��� �����ߴ��� ����  �޾ƾ���  
    // AttackType ���� ( attackSO ���� ) 
    public void SaveInputKey(KeyCode keyCode)
    {
        AttackData attackData = _skillComponent.GetAttackData(_skillInventorySO.skillList[(int)keyCode - 49].attackType);
        AttackType attackType = _skillInventorySO.skillList[(int)keyCode - 49].attackType;
        _inputModule.RegisterKeyAction(keyCode, () => _skillComponent.PlayAttackCallback(attackType));
        
    }

    /// <summary>
    /// Ű ������ �� ���� 
    /// </summary>
    /// <param name="keyCode"></param>
    public void RemoveInputKey(KeyCode keyCode)
    {
        Debug.Log(keyCode); 
        _inputModule.RemoveKeyAction(keyCode);
    }
}
