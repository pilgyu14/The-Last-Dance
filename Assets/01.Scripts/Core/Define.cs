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
}
