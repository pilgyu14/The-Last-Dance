using UnityEngine; 

[CreateAssetMenu(menuName = "SO/AgentSO/EnemySO")]
public class EnemySO : AgentSO
{
    [Header("AI"), Space(20)]

    [Header("거리 체크")]
    public float chaseDistance;  // 추적 거리 
    public float eyeAngle; // 시야각 
    
    public float attackDistance; // 기본 공격 거리 
}