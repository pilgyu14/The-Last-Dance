using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputModule : MonoBehaviour
{
    // üũ ���� 
    private bool _isPlayerInput;
    private bool _isUIInput;

    // ���� 
    private float _x;
    private float _y; 

    // ������Ƽ 
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

    }

    /// <summary>
    /// UI �Է� �ޱ� 
    /// </summary>

    private void UIInput()
    {

    }
}
