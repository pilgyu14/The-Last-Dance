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
    /// 액티브 스킬 등록 및 삭제 
    /// </summary>
    public void CheckActiveSkill()
    {
        // 키 등록되어 있는 것들 확인하고
        // 키에 등록되어 있는 attackBinds들 얻어서 
        // 스킬 인벤토리에 있는지 확인해서 없으면 
        // 키 등록되어 있는 거 삭제  

        for(int i =0;i < _inputModule.skillInputList.Count; i++)
        {
            InputBinding skillInput = _inputModule.skillInputList[i]; 
            bool isRemove = true;
            for (int j = 0; j < _skillInventorySO.skillList.Count; j++) // 스킬리스트에 있는 어택타입이 
            {
                AttackData attackData = _skillComponent.GetAttackData(_skillInventorySO.skillList[(int)skillInput.keyCode - 49].attackType); // 스킬 데이터 가져왔어

                if (attackData != null && attackData.callback == skillInput.callback) // 입력에 등록되어 있으면서 스킬인벤토리에도 있으면  삭제 안해 
                {
                    isRemove = false;
                }
            }
            if (isRemove == true) // 없었다면 삭제 
            {
                RemoveInputKey(skillInput.keyCode);
            }
        }


        // 스킬 리스트에 있는데
        // 키 등록 안되어 있으면 
        // 리스트 인덱스 + 49 로 키코드 가져와서 등록 
        for (int i = 0; i < _skillInventorySO.skillList.Count; i++) // 스킬리스트에 있는 어택타입이 
        {
            AttackData attackData = _skillComponent.GetAttackData(_skillInventorySO.skillList[i].attackType); // 현재 보유 스킬이 액티브 스킬들 중에 있는지 확인 

            KeyCode keyCode = (KeyCode)(i + 49);
            if (attackData != null && _inputModule.IsKeyRegister(keyCode) == false) // 스킬리스트에 있으면서 키 등록이 안되어 있다면 등록 하기 
            {
                SaveInputKey(keyCode);
            }
        }
    }

    // 어떤 키 어디에 저장했는지 정보  받아야해  
    // AttackType 정보 ( attackSO 에서 ) 
    public void SaveInputKey(KeyCode keyCode)
    {
        AttackData attackData = _skillComponent.GetAttackData(_skillInventorySO.skillList[(int)keyCode - 49].attackType);
        AttackType attackType = _skillInventorySO.skillList[(int)keyCode - 49].attackType;
        _inputModule.RegisterKeyAction(keyCode, () => _skillComponent.PlayAttackCallback(attackType));
        
    }

    /// <summary>
    /// 키 저장한 거 삭제 
    /// </summary>
    /// <param name="keyCode"></param>
    public void RemoveInputKey(KeyCode keyCode)
    {
        Debug.Log(keyCode); 
        _inputModule.RemoveKeyAction(keyCode);
    }
}
