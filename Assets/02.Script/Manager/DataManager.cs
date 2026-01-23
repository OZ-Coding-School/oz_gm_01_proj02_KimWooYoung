using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    [Header("Progress")]
    public PlayerProgressData progress;

    [Header("Skill Count")]
    [SerializeField] private int skillCount = 3;

    public int CurrentSlot { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitProgress();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitProgress()
    {
        progress = new PlayerProgressData
        {
            maxUnlockedStage = 1, // 스테이지 1 기본 해금
            skillUnlocked = new bool[skillCount]
        };
    }
    public void ClearStage(int clearedStage)
    {
        if (clearedStage >= progress.maxUnlockedStage)
        {
            progress.maxUnlockedStage = clearedStage + 1;
            SaveAllData(CurrentSlot);
        }
    }
    public void SaveAllData(int slotIndex)
    {
        string prefix = $"Save{slotIndex}_";

        PlayerPrefs.SetInt(prefix + "MaxStage", progress.maxUnlockedStage);

        for (int i = 0; i < progress.skillUnlocked.Length; i++)
        {
            PlayerPrefs.SetInt(
                prefix + $"Skill_{i}",
                progress.skillUnlocked[i] ? 1 : 0
            );
        }

        PlayerPrefs.Save();
    }

    public void LoadAllData(int slotIndex)
    {
        string prefix = $"Save{slotIndex}_";

        progress.maxUnlockedStage =
            PlayerPrefs.GetInt(prefix + "MaxStage", 1);

        for (int i = 0; i < progress.skillUnlocked.Length; i++)
        {
            progress.skillUnlocked[i] =
                PlayerPrefs.GetInt(prefix + $"Skill_{i}", 0) == 1;
        }
    }

}
