using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour, IComponent
{
    //에디터 확인용 변수 
    public float EyeRadius;
    public float EyeAngle; 

    [Header("Search Elements")]
    public float delayFindTime = 0.2f;

    public LayerMask targetLayerMask;
    public LayerMask blockLayerMask;

    private List<Transform> targetLists = new List<Transform>(); // 시약 각,범위 안에 잡힌 타겟들 
    [SerializeField]
    private Transform firstTarget;
    private float distanceTarget = 0.0f;

    private float findDelayTime = 0.2f;

    public Transform FirstTarget => firstTarget;
    public List<Transform> TargetList => targetLists;
    public float DistanceTarget => distanceTarget;

    /// <summary>
    /// 시야 범위 내에 적이 있으면 TargetList에 저장, true 반환 없으면 false반환  
    /// </summary>
    /// <param name="eyeAngle"></param>
    /// <param name="eyeRadius"></param>
    /// <returns></returns>
    public bool FindTargets(float eyeAngle, float eyeRadius)
    {
        EyeRadius = eyeRadius;
        EyeAngle = eyeAngle; 

        distanceTarget = 0.0f;
        firstTarget = null; // 가장 가까운 적 
        targetLists.Clear();

        Collider[] overlapSphereTargets = Physics.OverlapSphere(transform.position, eyeRadius, targetLayerMask); // 범위 안에 있는 타겟 
        for (int i = 0; i < overlapSphereTargets.Length; ++i)
        {
            Transform target = overlapSphereTargets[i].transform;

            Vector3 LookAtTarget = (target.position - transform.position).normalized; // 적을 바라보는 방향 벡터

            // 시야각 판별
            if (Vector3.Angle(transform.forward, LookAtTarget) < eyeAngle / 2)
            {
                float firstDistance = Vector3.Distance(transform.position, target.position);

                // 몬스터와 타겟 사이의 장애물 판정
                if (Physics.Raycast(transform.position, LookAtTarget, firstDistance, blockLayerMask) == false) // 적과 나 사이에 건물이 없다면 판별 
                {
                    targetLists.Add(target);

                    if (firstTarget == null || distanceTarget > firstDistance)
                    {
                        firstTarget = target;
                        distanceTarget = firstDistance;
                    }
                }

            }
            // 거리 판별 
        }

        return (firstTarget != null) ? true : false; // 찾았으면 treu 못찾았으면 false 
    }


    public Vector3 GetVecByAngle(float degrees, bool flagGlobalAngle)
    {
        if (flagGlobalAngle == false)
        {
            degrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(degrees * Mathf.Deg2Rad),
            0,
            Mathf.Cos(degrees * Mathf.Deg2Rad));
    }
}
