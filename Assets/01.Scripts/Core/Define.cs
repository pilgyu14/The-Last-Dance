using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    private static Camera _mainCam; 
    public static Camera MainCam
    {
        get
        {
            _mainCam ??= GameObject.FindObjectOfType<Camera>();
            return _mainCam; 
        }
    }

    public static Vector3 WorldMousePos
    { 
        get
        {
            Ray ray = Define.MainCam.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out RaycastHit hitInfo);
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 3f);

            return hitInfo.point; 


        }
    }
}
