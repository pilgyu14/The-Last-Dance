using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/AttackSO/SkillTable")]
public class SkillTableSO : ScriptableObject
{
    public List<AttackSO> skillTable;
}
