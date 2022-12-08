using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour, IComponent
{
    //������ Ȯ�ο� ���� 
    public float EyeRadius;
    public float EyeAngle; 

    [Header("Search Elements")]
    public float delayFindTime = 0.2f;

    public LayerMask targetLayerMask;
    public LayerMask blockLayerMask;

    private List<Transform> targetLists = new List<Transform>(); // �þ� ��,���� �ȿ� ���� Ÿ�ٵ� 
    [SerializeField]
    private Transform firstTarget;
    private float distanceTarget = 0.0f;

    private float findDelayTime = 0.2f;

    public Transform FirstTarget => firstTarget;
    public List<Transform> TargetList => targetLists;
    public float DistanceTarget => distanceTarget;

    /// <summary>
    /// �þ� ���� ���� ���� ������ TargetList�� ����, true ��ȯ ������ false��ȯ  
    /// </summary>
    /// <param name="eyeAngle"></param>
    /// <param name="eyeRadius"></param>
    /// <returns></returns>
    public bool FindTargets(float eyeAngle, float eyeRadius)
    {
        EyeRadius = eyeRadius;
        EyeAngle = eyeAngle; 

        distanceTarget = 0.0f;
        firstTarget = null; // ���� ����� �� 
        targetLists.Clear();

        Collider[] overlapSphereTargets = Physics.OverlapSphere(transform.position, eyeRadius, targetLayerMask); // ���� �ȿ� �ִ� Ÿ�� 
        for (int i = 0; i < overlapSphereTargets.Length; ++i)
        {
            Transform target = overlapSphereTargets[i].transform;

            Vector3 LookAtTarget = (target.position - transform.position).normalized; // ���� �ٶ󺸴� ���� ����

            // �þ߰� �Ǻ�
            if (Vector3.Angle(transform.forward, LookAtTarget) < eyeAngle / 2)
            {
                float firstDistance = Vector3.Distance(transform.position, target.position);

                // ���Ϳ� Ÿ�� ������ ��ֹ� ����
                if (Physics.Raycast(transform.position, LookAtTarget, firstDistance, blockLayerMask) == false) // ���� �� ���̿� �ǹ��� ���ٸ� �Ǻ� 
                {
                    targetLists.Add(target);

                    if (firstTarget == null || distanceTarget > firstDistance)
                    {
                        firstTarget = target;
                        distanceTarget = firstDistance;
                    }
                }

            }
            // �Ÿ� �Ǻ� 
        }

        return (firstTarget != null) ? true : false; // ã������ treu ��ã������ false 
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
