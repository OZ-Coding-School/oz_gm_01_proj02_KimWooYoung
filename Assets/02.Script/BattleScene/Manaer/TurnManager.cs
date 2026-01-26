using System.Collections;
using UnityEngine;

public enum TurnState {Start, PlayerTurn, EnemyTurn, Win, Lose}
public class TurnManager : MonoBehaviour
{
    [SerializeField] private MoveRangeVisualizer moveRangeVisualizer;

    [SerializeField] private IsometricCamera isometricCamera;
    [SerializeField] private Transform player;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private PlayerAttack playerAttack;

    public TurnState state;

    private void Awake()
    {
        GameManager.Instance.SetTurnManager(this);
    }
    private void Start()
    {
        state = TurnState.Start;
        StartCoroutine(PlayerTurn());
    }
    IEnumerator PlayerTurn()
    {
        state = TurnState.PlayerTurn;

        playerAttack.RestTurn();

        if (player == null || !player.gameObject.activeSelf)
        {
            yield break;
        }

        isometricCamera.SetTarget(player);


        moveRangeVisualizer.ShowMoveRage();

        yield return new WaitUntil(() => moveRangeVisualizer.IsMoving);

        yield return new WaitUntil(() => !moveRangeVisualizer.IsMoving);

        uiManager.AttackUIOpen();

        yield return new WaitUntil(() => playerAttack.HasConfirmedAttack);

        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnemyTurn()
    {

        state = TurnState.EnemyTurn;

        var enemies = GameManager.Instance.GetEnemies();

        if (enemies.Count == 0)
        {
            SetState(TurnState.Win);
            yield break;
        }

        foreach (EnemyFSM enemy in enemies) 
        {
            if (enemy == null || !enemy.gameObject.activeSelf)
            continue;

            isometricCamera.SetTarget(enemy.transform);

            yield return StartCoroutine(enemy.StartTurnCoroutine());

            yield return new WaitForSeconds(0.5f);


        }

        StartCoroutine(PlayerTurn());
    }

    public void SetState(TurnState newState)
    {
        state = newState;
        StopAllCoroutines();

        switch (state)
        {
            case TurnState.Win:
                OnWin();
                break;
            case TurnState.Lose:
                OnLose();
                break;
        }
    }

    public void EndPlayerTunr()
    {
        if (state != TurnState.PlayerTurn)
            return;
        StopAllCoroutines();
        StartCoroutine(EnemyTurn());
    }

    private void OnWin()
    {
        int clearedStageIndex = DataManager.Instance.maxUnlockedStage;
        DataManager.Instance.HandleStageClear(clearedStageIndex);
        uiManager.GameCler();
 
    }

    private void OnLose()
    {
        uiManager.GameOver();
    }
}
