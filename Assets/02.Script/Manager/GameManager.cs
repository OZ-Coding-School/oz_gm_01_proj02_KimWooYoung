using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : Singleton<GameManager>   
{
    [SerializeField] private TurnManager turnManager;

    private List<EnemyFSM> enemies = new List<EnemyFSM>();

    protected override void Init()
    {
       base.Init();
        enemies.Clear();
        enemies.AddRange(FindObjectsOfType<EnemyFSM>());
    }

    public void SetTurnManager(TurnManager tm)
    {
        turnManager = tm;
    }

    public void RegisterEnemy(EnemyFSM enemy)
    {
        if(!enemies.Contains(enemy)) enemies.Add(enemy);
    }

    public void UnregisterEnemy(EnemyFSM enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }
        enemies.RemoveAll(e => e == null);

        if(enemies.Count == 0)
        {
            Debug.Log("Stage Clear"); 

            turnManager.SetState(TurnState.Win);
        }
    }
    public List<EnemyFSM> GetEnemies()
    {
        enemies.RemoveAll(e => e == null || !e.gameObject.activeSelf);
        return enemies;
    }

    //스테이지 해금 처리

}
