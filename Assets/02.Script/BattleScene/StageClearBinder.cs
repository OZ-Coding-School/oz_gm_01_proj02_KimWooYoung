using UnityEngine;

public class StageClearBinder : MonoBehaviour
{
    [SerializeField] private int stageIndex;

    private void OnEnable()
    {
        GameManager.Instance.OnStageCleared += HandleStageClear;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnStageCleared -= HandleStageClear;
    }

    private void HandleStageClear()
    {
        DataManager.Instance.HandleStageClear(stageIndex);
    }
}