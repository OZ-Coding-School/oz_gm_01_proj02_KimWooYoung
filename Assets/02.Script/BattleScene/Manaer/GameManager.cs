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
    }

    public void RegisterEnemy(EnemyFSM enemy)
    {
        if(!enemies.Contains(enemy)) enemies.Add(enemy);
    }

    public void UnregisterEnemy(EnemyFSM enemy)
    { 
        enemies.Remove(enemy);
        if(enemies.Count == 0)
        {
            Debug.Log("Stage Clear"); 

            turnManager.SetState(TurnState.Win);
        }
    }
    //스테이지 해금 처리

}
