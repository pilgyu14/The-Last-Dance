using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAgent
{
    public bool IsDie { get; set; }
    public float HP { get;  set; }

    public void OnDie(); 
}
