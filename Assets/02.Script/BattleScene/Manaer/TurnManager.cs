using System.Collections;
using UnityEngine;

public enum TurnState {Start, PlayerTurn, EnemyTurn, Win, Lose}
public class TurnManager : MonoBehaviour
{
    [SerializeField] private MoveRangeVisualizer moveRangeVisualizer;

    [SerializeField] private IsometricCamera isometricCamera;
    [SerializeField] private Transform player;
    [SerializeField] private EnemyFSM enemyFSM;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private PlayerAttack playerAttack;

    public TurnState state;

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
        if (enemyFSM == null || !enemyFSM.gameObject.activeSelf)
        {
            StartCoroutine(PlayerTurn());
            yield break;
        }

        state = TurnState.EnemyTurn;

        isometricCamera.SetTarget(enemyFSM.transform);
        Debug.Log("Àû ÅÏ");

        enemyFSM.StartTrun();
        yield return new WaitUntil(() => enemyFSM.IsMoving);

        yield return new WaitUntil(() => !enemyFSM.IsMoving);

        yield return new WaitForSeconds(0.5f);

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
        uiManager.GameCler();
    }

    private void OnLose()
    {
        uiManager.GameOver();
    }
}
