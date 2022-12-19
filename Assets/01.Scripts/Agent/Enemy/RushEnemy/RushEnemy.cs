using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushComponent <T>
{
    private T _owner;
    private Material _mat; 
    private AgentMoveModule _moveModule;

    public void Init(T owner, Material mat, AgentMoveModule moveModule)
    {
        
    }
}

public class RushEnemy : Enemy
{
    private RushEnemyTree _rushEnemyTree;

    private Material _mat;
    [SerializeField]
    private Color _rushChangeColor;
    [SerializeField]
    private Color _originColor;

    protected override void Awake()
    {
        base.Awake();
        _mat = _enemyAnimation.GetComponentInChildren<SkinnedMeshRenderer>().material;
    }

    protected override void Start()
    {
        base.Start();
        _originColor = _mat.GetColor("_EmissionColor");
    }

    protected override void CreateTree()
    {
        _rushEnemyTree = new RushEnemyTree(this);
    }

    protected override void UpdateTree()
    {
        _rushEnemyTree.UpdateRun();
    }

    #region Condition

    public bool CheckRushCoolTime()
    {
        Debug.Log("���� ��Ÿ�� üũ");
        return _attackModule.GetAttackInfo(AttackType.RushAttack).IsCoolTime;
    }
    /// <summary>
    /// ���� ���� ���� üũ 
    /// </summary>
    /// <returns></returns>
    public bool CheckRushAttack()
    {
        Debug.Log("���� ���� üũ");
        float atkDistance = _attackModule.GetAttackInfo(AttackType.RushAttack).attackInfo.attackSO.attackRadius;
        return CheckDistance(_enemySO.eyeAngle, atkDistance);
    }

    #endregion
    /// <summary>
    /// ���� ����
    /// </summary>
    public void RushAttack()
    {
        _isAttacking = true; // �����־�� �ϱ⿡ 
        _moveModule.StopMove();
        StartCoroutine(ChangeColor());

    }

    /// <summary>
    /// ���� �غ��ڼ����� �� �Ͼ�� ���� 
    /// </summary>
    /// <returns></returns>
    IEnumerator ChangeColor()
    {
        _mat.EnableKeyword("_EMISSION");
        Color newColor = new Color(0, 0, 0);
        while (newColor.r < 0.7f)
        {
            LookTarget();
            newColor = Color.Lerp(newColor, _rushChangeColor, Time.deltaTime);
            _mat.SetColor("_EmissionColor", newColor);

            yield return null;
        }
        yield return new WaitForSeconds(0.1f);

        DefaultAttack(AttackType.RushAttack); // �� ���� �� ����!

    }

    /// <summary>
    /// �� �������� ( �ִϸ��̼� ������ �� ) 
    /// </summary>
    public void InitColor()
    {
        Debug.Log("�� �ʱ�ȭ");
        _mat.SetColor("_EmissionColor", _originColor);
    }

    /// <summary>
    /// ���� �̵� ( Attack Feedback ) 
    /// </summary>
    public void RushMove()
    {
        Vector3 dir = (_target.position - transform.position).normalized;
        float rushPower = _attackModule.GetAttackInfo(AttackType.RushAttack).attackInfo.attackSO.rushPower;
        StartCoroutine(_moveModule.DashCorutine(dir, rushPower, 0.2f));

        // _moveModule.DashCorutine(dir, knockbackPower, 0.2f);
    }

    public override void Reset()
    {
        base.Reset();
        InitColor(); // �� �ʱ�ȭ 
    }
}
