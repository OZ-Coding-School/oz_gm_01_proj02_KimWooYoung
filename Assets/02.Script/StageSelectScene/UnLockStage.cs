using UnityEngine;

public class UnlockStage : MonoBehaviour
{
    [SerializeField] private int stageIndex;

    public Sprite lockSprite;
    public Sprite unlockSprite;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        bool unlocked =
            stageIndex <= DataManager.Instance.progress.maxUnlockedStage;

        sr.sprite = unlocked ? unlockSprite : lockSprite;
    }
}
