using System.Collections;
using UnityEngine;

public enum TurnState {Start, PlayerTurn, EnemyTurn, Win, Lose}
public class TurnManager : MonoBehaviour
{
    [SerializeField] private MoveRangeVisualizer moveRangeVisualizer;

    [SerializeField] private IsometricCamera isometricCamera;
    [SerializeField] private Transform player;
    [SerializeField] private EnemyFSM enemyFSM;

    public TurnState state;    

    private void Start()
    {
        state = TurnState.Start;
        StartCoroutine(PlayerTurn());
    }
    IEnumerator PlayerTurn()
    {
        state = TurnState.PlayerTurn;

        isometricCamera.SetTarget(player);

        moveRangeVisualizer.ShowMoveRage();

        yield return new WaitUntil(() => moveRangeVisualizer.IsMoving);

        yield return new WaitUntil(() => !moveRangeVisualizer.IsMoving);


        var attack = FindObjectOfType<PlayerAttack>();
        if (attack != null)
            yield return new WaitUntil(() => !attack.IsAttacking);

        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnemyTurn()
    {
        state = TurnState.EnemyTurn;

        isometricCamera.SetTarget(enemyFSM.transform);
        Debug.Log("Àû ÅÏ");

        enemyFSM.StartTrun();
        yield return new WaitUntil(() => enemyFSM.IsMoving);

        yield return new WaitUntil(() => !enemyFSM.IsMoving);

        StartCoroutine(PlayerTurn());
    }


}
