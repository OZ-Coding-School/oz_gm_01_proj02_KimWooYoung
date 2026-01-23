
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    [SerializeField] private int skillIndex;
    [SerializeField] private SkillSO skill;
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private UIManager uIManager;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }
    public void Refresh()
    {
        if (DataManager.Instance == null)
            return;

        if (skillIndex < 0 || skillIndex >= DataManager.Instance.skillUnlocked.Length)
        {
            Debug.LogError($"{name} : skillIndex {skillIndex} 범위 초과");
            button.interactable = false;
            return;
        }

        bool unlocked = DataManager.Instance.IsSkillUnlocked(skillIndex);
        button.interactable = unlocked;
    }
    public void OnClickSkill()
    {

        if (!DataManager.Instance.IsSkillUnlocked(skillIndex))
        {
            Debug.Log($"Skill {skillIndex} 잠김");
            return;
        }


        playerAttack.SetSkill(skill);

        playerAttack.ShowAttackRange(true);

        uIManager.SkillPanelClose();
    }
}

