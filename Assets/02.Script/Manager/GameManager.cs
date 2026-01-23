using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>   
{
    [SerializeField] private TurnManager turnManager;

    private List<EnemyFSM> enemies = new List<EnemyFSM>();
    private bool stageCleared = false;

    public event Action OnStageCleared;

    protected override void Init()
    {
       base.Init();
        enemies.Clear();
        enemies.AddRange(FindObjectsOfType<EnemyFSM>());
        stageCleared = false;

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
        if (stageCleared) return;

        enemies.Remove(enemy);
        enemies.RemoveAll(e => e == null);

        if(enemies.Count == 0)
        {

            stageCleared = true;

            Debug.Log("Stage Clear");

            OnStageCleared?.Invoke();


            turnManager.SetState(TurnState.Win);
        }
    }
    public List<EnemyFSM> GetEnemies()
    {
        enemies.RemoveAll(e => e == null || !e.gameObject.activeSelf);
        return enemies;
    }

}
