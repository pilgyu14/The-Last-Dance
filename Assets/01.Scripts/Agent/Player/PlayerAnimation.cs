using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : AgentAnimation
{
    // Hash

    // ����
    public readonly int _battleHash = Animator.StringToHash("Battle");

    // �̵� 
    public readonly int _velocityXHash = Animator.StringToHash("VelocityX");
    public readonly int _velocityYHash = Animator.StringToHash("VelocityZ");

    // ���� 
  
    // �⺻ ���� 
    private readonly int _frontKickHash = Animator.StringToHash("FrontKick");
    private readonly int _sideKickHash = Animator.StringToHash("SideKick");
    private readonly int _backKickHash = Animator.StringToHash("BackKick");

    private readonly int _tackleHash = Animator.StringToHash("Tackle");
    private readonly int _hurricaneKickHash = Animator.StringToHash("HurricaneKick"); 

    // Hash ��

    private List<string> _defaultAttackStrList = new List<string>(); // �⺻ ���� �ִϸ��̼� �ؽ� ����Ʈ  
    protected override void Awake()
    {
        base.Awake(); 
    }

    private void Start()
    {
        SetHashList(); 
    }

    /// <summary>
    /// �ִϸ޾Ƽ� �ؽ�����Ʈ�� �ֱ� 
    /// </summary>
    private void SetHashList()
    {
        _defaultAttackStrList.Add("FrontKick");
        _defaultAttackStrList.Add("SideKick");
        _defaultAttackStrList.Add("BackKick");
    }

    /// <summary>
    /// �⺻ ���� �ִϸ��̼����� üũ
    /// </summary>
    public bool CheckDefaultAnim()
    {
        foreach(var h in _defaultAttackStrList)
        {
            bool b = _agentAnimator.GetCurrentAnimatorStateInfo(0).IsName(h);
            if (b == true)
            {
                Debug.Log("A####################3");
                return true;
            }
        }
        return false; 
    }

    // ���� ���� 
    public void SetBattle(bool active)
    {
        _agentAnimator.SetBool(_battleHash, active); 
    }

    // ű �ִϸ��̼� (AttackSO���� string���� �ް� ���÷������� ���� ) 
    public void SetFrontKick()
    {
        _agentAnimator.SetTrigger(_frontKickHash);
    }
    public void SetSideKick()
    {
        _agentAnimator.SetTrigger(_sideKickHash); 
    }
    public void SetBackKick()
    {
        _agentAnimator.SetTrigger(_backKickHash);
    }

    public void SetTackle()
    {
        _agentAnimator.SetTrigger(_tackleHash);
    }

    public void PlayHurricaneKick(bool isActive)
    {
        _agentAnimator.SetBool(_hurricaneKickHash, isActive);
    }

    // �̵� ���� 
    public void SetVelocity(Vector3 v)
    {
        _agentAnimator.SetFloat(_velocityXHash, v.x);
        _agentAnimator.SetFloat(_velocityYHash, v.z);
    }

}
