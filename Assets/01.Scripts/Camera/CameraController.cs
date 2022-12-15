using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private CinemachineFreeLook cam;

    [SerializeField]
    private float _minZoom = 7f;
    [SerializeField]
    private float _maxZoom = 40f;
    [SerializeField]
    private float lookPlayerFovV; // �÷��̾ �ٷΰ�
    [SerializeField]
    private float _wheelSpeed = 5f;


    private float Fov
    {
        get => cam.m_Lens.FieldOfView;
        set => cam.m_Lens.FieldOfView = value; 
    }

    private void Awake()
    {
        cam = GetComponent<CinemachineFreeLook>();
    }

    private void Start()
    {
        Fov = _maxZoom;
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

        CamZoomInOut(); 

    }

    [SerializeField]
    float scroll; 
    // ī�޶� Ȯ�� ��� 
    private void CamZoomInOut()
    {
         scroll = - Input.GetAxis("Mouse ScrollWheel") * _wheelSpeed;

        if(Fov <= _minZoom && scroll < 0) // �ּ� �� üũ
        {
            Fov = _minZoom; 
        }
        else if (Fov >= _maxZoom && scroll > 0) // �ִ� �� üũ
        {
            Fov = _maxZoom;
        }
        else // FOV ���� 
        {
            Fov = Mathf.Lerp(Fov, Fov + scroll * _wheelSpeed, Time.deltaTime );
        }

      //  if(GameManager.Instance.PlayerTrm && Fov <= )
    }
}
