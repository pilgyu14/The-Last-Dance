using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private CinemachineFreeLook cam;

    private void Awake()
    {
        cam = GetComponent<CinemachineFreeLook>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            cam.m_XAxis.m_InputAxisName = "Mouse X";
            cam.m_YAxis.m_InputAxisName = "Mouse Y";
        }

        if (Input.GetMouseButtonUp(1))
        {
            cam.m_XAxis.m_InputAxisName = "";
            cam.m_YAxis.m_InputAxisName = "";

            cam.m_YAxis.m_InputAxisValue = 0f;
            cam.m_XAxis.m_InputAxisValue = 0f;


        }
    }
}
