using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        //FieldOfView fov = (FieldOfView)target;

        //Handles.color = Color.yellow;
        //Handles.DrawWireArc(fov.transform.position, Vector2.up, Vector3.forward, 360, fov.EyeRadius);

        //float rightAngleX = Mathf.Sin((fov.EyeAngle / 2) * Mathf.Deg2Rad) * fov.EyeRadius;
        //float angleZ = Mathf.Cos((fov.EyeAngle / 2) * Mathf.Deg2Rad) * fov.EyeRadius;
        //Vector3 vecRigiht = new Vector3(rightAngleX, 0, angleZ);

        //float leftAngleX = Mathf.Sin(-fov.EyeAngle / 2) * Mathf.Deg2Rad * fov.EyeRadius;
        //Vector3 vecLeft = new Vector3(leftAngleX, 0, angleZ);

        //// Handles.DrawLine(fov.transform.position, fov.transform.position + vecRigiht,3f);
        //// Handles.DrawLine(fov.transform.position, fov.transform.position + vecLeft, 3f);

        //Vector3 viewRight = fov.GetVecByAngle(fov.EyeAngle / 2, false);
        //Vector3 viewLeft = fov.GetVecByAngle(-fov.EyeAngle / 2, false);

        //Handles.DrawLine(fov.transform.position, fov.transform.position + viewRight * fov.EyeRadius);
        //Handles.DrawLine(fov.transform.position, fov.transform.position + viewLeft * fov.EyeRadius);

        //// 시야에 들어온 모든 타겟 
        //Handles.color = Color.red;
        //foreach (Transform visibleTarget in fov.TargetList)
        //{
        //    if (fov.FirstTarget != visibleTarget)
        //        Handles.DrawLine(fov.transform.position, visibleTarget.position);
        //}

        //// 공격할 타겟은 초록색으로 
        //if (fov.FirstTarget != null)
        //{
        //    Handles.color = Color.green;
        //    Handles.DrawLine(fov.transform.position, fov.FirstTarget.position);

        //}
    }
}
