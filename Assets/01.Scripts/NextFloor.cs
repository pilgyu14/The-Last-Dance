using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextFloor : MonoBehaviour
{
    [SerializeField]
    private string nextFloorName = "";

    private bool isTouch = false;

    private void Update()
    {
        if (isTouch == true)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                GoNextFloor();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance.CanNextFloor())
            {
                isTouch = true;
                ItemUI.Instance.pickupNameText.text = "���� ������";
            }
            else
                ItemUI.Instance.pickupNameText.text = "�ڰ��� �����ϴ�.";
            ItemUI.Instance.pickup.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isTouch = isTouch ? false : false;
            ItemUI.Instance.pickup.SetActive(false);
        }
    }

    private void GoNextFloor()
    {
        SceneManager.LoadScene(nextFloorName);
    }
}
