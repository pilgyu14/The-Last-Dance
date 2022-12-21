using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeEffectAndAttack
{
    public AttackType attackType;
    public int upgradeDamage; 
    public EffectComponent effectComponent;
    public Color[] originColor; 
    public Color[] upgradeColor;

    public void UpgradeColor()
    {
        ParticleSystem.MainModule psMain = effectComponent.Particle.main;
        psMain.startColor = upgradeColor[0];


        ParticleSystem.MainModule psMain2 = effectComponent.ParticleList[0].main;
        psMain2.startColor = upgradeColor[1];
    }

    public void DowngradeColor()
    {
        ParticleSystem.MainModule psMain = effectComponent.Particle.main;
        psMain.startColor = originColor[0];

        ParticleSystem.MainModule psMain2 = effectComponent.ParticleList[0].main;
        psMain2.startColor = originColor[1];
    }
}


public class DefaultAttackUpgradeSkill : SkillData<PlayerController>
{
    [SerializeField]
    private List<UpgradeEffectAndAttack> _effectList = new List<UpgradeEffectAndAttack>();

    public DefaultAttackUpgradeSkill(PlayerController playerController)
    {
        this._owner = playerController; 
    }
    public override void Enter()
    {
        UpgradeEffectColor(); 
    }

    public override void Stay()
    {

    }

    public override void Exit()
    {
        DowngradeEffectColor(); 
    }
    private void UpgradeEffectColor()
    {
        for(int i=0;i < _effectList.Count; i++)
        {
            _effectList[i].UpgradeColor();
            _owner.AttackModule.GetAttackInfo(_effectList[i].attackType).attackInfo.attackSO.attackDamage += _effectList[i].upgradeDamage; 
        }
    }

    private void DowngradeEffectColor()
    {
        for (int i = 0; i < _effectList.Count; i++)
        {
            _effectList[i].DowngradeColor();
            _owner.AttackModule.GetAttackInfo(_effectList[i].attackType).attackInfo.attackSO.attackDamage -= _effectList[i].upgradeDamage;
        }
    }

  
}
