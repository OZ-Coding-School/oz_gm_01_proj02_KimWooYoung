using System.Collections;
using UnityEngine;

public enum TurnState {START, PLAYERTURN, ENEMYTURN, WON, LOST}
public class TurnManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public TurnState turnState;
    

    private void Start()
    {
        turnState = TurnState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        yield return new WaitForSeconds(2f);

        turnState = TurnState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        yield return new WaitForSeconds(2f);
    }
    private void PlayerTurn()
    {

    }

    void OnAttackButton()
    {
        if (turnState != TurnState.PLAYERTURN) return;

        StartCoroutine( PlayerAttack());

    }
}
