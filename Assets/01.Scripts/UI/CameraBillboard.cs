using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBillboard : MonoBehaviour
{
    [SerializeField]
    private bool reverseFase = false;


    Vector3 targetPos;
    Vector3 targetOrientation;
    private void LateUpdate()
    {
        targetPos = transform.position + Define.MainCam.transform.rotation * (reverseFase ? Vector3.forward : Vector3.back);
        targetOrientation = Define.MainCam.transform.rotation * Vector3.up;
        transform.LookAt(targetPos, targetOrientation); 
    }
}
