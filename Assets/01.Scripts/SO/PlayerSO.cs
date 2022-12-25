using UnityEngine;

[CreateAssetMenu(menuName = "SO/AgentSO/PlayerSO")]
public class PlayerSO : AgentSO
{
    // 능력치 = (기준 스탯 * 레벨) 형식의ㅡ 식 
    // hp = defaultHP * level ... 
    [Header("기본 기준스탯")]
    public int defaultHP; // HP 증가 계수 
    public int baseHp; // 기본 HP 

    public int defaultMaxSpeed; // 스피드 증가 계수 
    public int baseMaxSpeed; // 기본 스피드

    [Header("레벨 관련")]
    public int exp; // 경험치 
    public int maxExp; // 

    public void Init()
    {
        level = 1;
        UpdateStat(); 
    }

    /// <summary>
    /// 레벨에 맞게 스탯 업데이트 
    /// </summary>
    public void UpdateStat()
    {
        maxHp = baseHp + defaultHP * level;
        moveInfo.maxSpeed = baseMaxSpeed + defaultMaxSpeed * level; 
    }

    public void CalculateExp(int monsterLevel)
    {
        EventManager.Instance.TriggerEvent(EventsType.UpdateExpUI,this.exp); // UI 업데이트 

        int calLevel = level - monsterLevel; // 레벨차 
        if(calLevel >= 0) // 플레이어가 유리 할 때 
        {

        }
        else // 플레이어가 불리할 때 
        {

        }
        int exp = (int) (calLevel * 10 * Random.Range(0.85f, 1f));
        exp = (2 / (calLevel + 2)) + 1; 
        // y = ((-20) / (x + 1)) + 21;
        UpdateExp(exp); 
    }
    /// <summary    >
    /// 경험치 체크해서 다찼으면 레벨업 
    /// </summary>
    public void UpdateExp(int newExp )
    {
        this.exp += newExp; 
        if(exp >= maxExp)
        {
            int overExp = exp - maxExp; // 넘는 경험치 받아두고 
            exp = overExp; 
            level++;
         //   maxExp = Mathf.Log(level)
        }
    }
}