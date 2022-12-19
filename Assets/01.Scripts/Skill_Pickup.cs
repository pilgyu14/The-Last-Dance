using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Pickup : PoolableMono
{
    private bool isTouch = false;
    

    void Update()
    {
        if (isTouch == true)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isTouch = true;
            
            ItemUI.Instance.pickup.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isTouch = false;
            ItemUI.Instance.pickup.SetActive(false);
        }
    }

    private void SkillAdd()
    {

    }

    public override void Reset()
    {
        
    }
}
