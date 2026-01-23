using UnityEngine;
using UnityEngine.UI;

public class UnLockStage : MonoBehaviour
{
    [SerializeField] private int stageIndex;

    private Image image;
    private Button button;

    private void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (DataManager.Instance == null)
            return;

        bool unlocked =
            stageIndex <= DataManager.Instance.maxUnlockedStage;

        image.color = unlocked ? Color.white : Color.gray;
        button.interactable = unlocked;
    }

}
