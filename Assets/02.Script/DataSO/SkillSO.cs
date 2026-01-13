using UnityEngine;

public enum TargetType
{
    Single,
    Area,
    Heal
}

[CreateAssetMenu(fileName = "SkillData", menuName = "Data/SkillData")]
public class SkillSO : ScriptableObject
{
    public string skillName;
    public int range;
    public int damage;
    public int areaSize;
    
    public TargetType type;
}
