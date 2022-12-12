using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_PickUp : MonoBehaviour
{
    private bool isTouch = false;

    private void Update()
    {
        if(isTouch == true)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                GameManager.Instance.pickupUI.SetActive(false);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isTouch = true;
            GameManager.Instance.pickupUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isTouch = false;
            GameManager.Instance.pickupUI.SetActive(false);
        }
    }
}
