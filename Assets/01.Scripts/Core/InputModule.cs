using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InputBinding
{
    
}


public class InputModule : MonoBehaviour
{
    // üũ ���� 
    private bool _isPlayerInput = true;
    private bool _isUIInput = true;

    // ���� 
    private float _x;
    private float _y;
    private Vector3 _moveDir;

    // ������Ƽ 
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
    /// Player �Է� �ޱ� 
    /// </summary>

    private void PlayerInput()
    {
        _x = Input.GetAxisRaw("Horizontal");
        _y = Input.GetAxisRaw("Vertical");
        
        _moveDir = new Vector3(_x, 0, _y).normalized; 
 
    }

    /// <summary>
    /// UI �Է� �ޱ� 
    /// </summary>

    private void UIInput()
    {

    }
}
