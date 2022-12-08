using UnityEngine; 

[CreateAssetMenu(menuName = "SO/AgentSO/EnemySO")]
public class EnemySO : AgentSO
{
    [Header("AI"), Space(20)]

    [Header("거리 체크")]
    public float chaseDistance;  // 추적 거리 
}