using UnityEngine;

[CreateAssetMenu(menuName = "SO/AgentSO/PlayerSO")]
public class PlayerSO : AgentSO
{
    // 능력치 = (기준 스탯 * 레벨) 형식의ㅡ 식 
    // hp = defaultHP * level ... 
    [Header("기본 기준스탯")]
    public int defaultHP;
    public int defaultMaxSpeed;

    [Header("레벨 관련")]
    public int exp; // 경험치 
    public int maxExp; // 
    public int level; // 레벨 

    /// <summary>
    /// 레벨에 맞게 스탯 업데이트 
    /// </summary>
    public void UpdateStat()
    {
        hp = defaultHP * level;
        moveInfo.maxSpeed = defaultMaxSpeed * level; 
    }

    /// <summary>
    /// 경험치 체크해서 다찼으면 레벨업 
    /// </summary>
    public void CheckeExp(int newExp )
    {
        this.exp += newExp; 
        if(exp >= maxExp)
        {
            int overExp = exp - maxExp; // 넘는 경험치 받아두고 
            exp = overExp; 
            level++; 
        }
    }
}