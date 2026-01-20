
using UnityEngine;

public class SkillButton : MonoBehaviour
{
    [SerializeField] private SkillSO skill;
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private UIManager uIManager;

    public void OnClickSkill()
    {
        playerAttack.SetSkill(skill);

        playerAttack.ShowAttackRange(true);

        uIManager.SkillPanelClose();
    }
}

