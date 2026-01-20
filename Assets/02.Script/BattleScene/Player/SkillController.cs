using System.Collections.Generic;
using UnityEngine;

public enum skillMenu { FireBoob, DarkOra, Lighting, WatherBob, Heal }


public class SkillController : MonoBehaviour
{
    [SerializeField] private List<SkillSO> skillSO = new List<SkillSO>();

    public skillMenu menu;

    public void SkillState(skillMenu skillList)
    {
        menu = skillList;

    }
}
