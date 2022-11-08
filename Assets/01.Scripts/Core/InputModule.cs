using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputModule : MonoBehaviour
{
    // 체크 변수 
    private bool _isPlayerInput;
    private bool _isUIInput;

    // 변수 
    private float _x;
    private float _y; 

    // 프로퍼티 
    public bool IsPlayerInput => _isPlayerInput;
    public bool IsUIInputInput => _isUIInput;

    private void Update()
    {
        if(_isPlayerInput == true)
        {
            PlayerInput(); 
        }
        if(_isUIInput == true)
        {
            UIInput(); 
        }
        
    }

    /// <summary>
    /// Player 입력 받기 
    /// </summary>

    private void PlayerInput()
    {

    }

    /// <summary>
    /// UI 입력 받기 
    /// </summary>

    private void UIInput()
    {

    }
}
