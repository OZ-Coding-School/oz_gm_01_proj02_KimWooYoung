using System.Collections;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public enum TurnState {Start, PlayerTurn, EnemyTurn, Win, Lose}
public class TurnManager : MonoBehaviour
{
    [SerializeField] private MoveRangeVisualizer moveRangeVisualizer;
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
        moveRangeVisualizer.ShowMoveRage();

        yield return new WaitUntil(() => moveRangeVisualizer.IsMoving);

        yield return new WaitUntil(() => !moveRangeVisualizer.IsMoving);

        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnemyTurn()
    {
        state = TurnState.EnemyTurn;
        Debug.Log("Àû ÅÏ");

        enemyFSM.StartTrun();
        yield return new WaitUntil(() => enemyFSM.IsMoving);

        yield return new WaitUntil(() => !enemyFSM.IsMoving);

        StartCoroutine(PlayerTurn());
    }


}
