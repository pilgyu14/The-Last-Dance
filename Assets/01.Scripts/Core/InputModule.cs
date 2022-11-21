using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InputBinding
{
    
}


public class InputModule : MonoBehaviour
{
    // 체크 변수 
    private bool _isPlayerInput = true;
    private bool _isUIInput = true;

    // 변수 
    private float _x;
    private float _y;
    private Vector3 _moveDir;

    // 프로퍼티 
    public Vector3 MoveDir => _moveDir; 
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
        _x = Input.GetAxisRaw("Horizontal");
        _y = Input.GetAxisRaw("Vertical");
        
        _moveDir = new Vector3(_x, 0, _y).normalized; 
 
    }

    /// <summary>
    /// UI 입력 받기 
    /// </summary>

    private void UIInput()
    {

    }
}
