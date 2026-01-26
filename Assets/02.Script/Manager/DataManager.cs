using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageReward
{
    public int stageIndex;
    public int unlockSkillIndex = -1; // -1이면 없음
}

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    public int CurrentSlot { get; private set; } = 0;

    [Header("Stage Progress")]
    public int maxUnlockedStage = 1;

    [Header("Skills")]
    public SkillSO[] skills;
    public bool[] skillUnlocked;

    [Header("Stage Rewards")]
    public List<StageReward> stageRewards = new List<StageReward>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitSkillData();

        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void InitSkillData()
    {
        if (skillUnlocked == null || skillUnlocked.Length != skills.Length)
        {
            skillUnlocked = new bool[skills.Length];
        }
    }

    public void HandleStageClear(int stageIndex)
    {

        bool changed = false;

        // 스테이지 해금
        if (stageIndex >= maxUnlockedStage)
        {
            maxUnlockedStage = stageIndex + 1;
            changed = true;
        }

        // 특정 스테이지 스킬 해금
        foreach (var reward in stageRewards)
        {
            if (reward.stageIndex == stageIndex &&
                reward.unlockSkillIndex >= 0 &&
                !skillUnlocked[reward.unlockSkillIndex])
            {
                skillUnlocked[reward.unlockSkillIndex] = true;
                changed = true;
            }
        }

        if (changed)
            SaveCurrentSlot();
    }
    public void SetCurrentSlot(int slot)
    {
        CurrentSlot = slot;
        PlayerPrefs.SetInt("LastSaveSlot", slot);
        PlayerPrefs.Save();

    }

    public bool HasSaveSlot(int slotIndex)
    {
        return PlayerPrefs.HasKey($"Save{slotIndex}_MaxStage");
    }

    public void SaveCurrentSlot()
    {
        if (CurrentSlot <= 0)
        {
            Debug.LogWarning("[DataManager] 저장 실패 - 슬롯 미선택");
            return;
        }

        SaveAllData(CurrentSlot);

        Debug.Log($"[DataManager] 슬롯 {CurrentSlot} 저장 완료");
    }

    private void SaveAllData(int slotIndex)
    {
        string prefix = $"Save{slotIndex}_";

        PlayerPrefs.SetInt(prefix + "MaxStage", maxUnlockedStage);

        for (int i = 0; i < skillUnlocked.Length; i++)
        {
            PlayerPrefs.SetInt(
                prefix + $"Skill_{i}",
                skillUnlocked[i] ? 1 : 0
            );
        }

        PlayerPrefs.Save();
    }
    public void LoadAllData(int slotIndex)
    {
        string prefix = $"Save{slotIndex}_";

        maxUnlockedStage =
            PlayerPrefs.GetInt(prefix + "MaxStage", 1);

        for (int i = 0; i < skillUnlocked.Length; i++)
        {
            skillUnlocked[i] =
                PlayerPrefs.GetInt(prefix + $"Skill_{i}", 0) == 1;
        }

        CurrentSlot = slotIndex;

        Debug.Log($"[DataManager] 슬롯 {slotIndex} 불러오기 완료");
    }

    public bool IsSkillUnlocked(int index)
    {
        if (index < 0 || index >= skillUnlocked.Length) return false;
        return skillUnlocked[index];
    }

}
