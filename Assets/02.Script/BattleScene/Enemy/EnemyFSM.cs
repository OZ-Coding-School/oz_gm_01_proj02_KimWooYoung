using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    public enum State
    {
        Idle,
        Chase,
        Attack
    }

    [SerializeField] private Transform target;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private AStarPathfinder aStar;
    [SerializeField] private EnemyAttack enemyAttack;
    //[SerializeField] private EnemySO EnemySO;

    [SerializeField] private int moveRange = 3;
    private float moveSpeed = 3f;

    public State CurrentState { get; private set; } = State.Idle;

    public bool IsMoving => isMoving;
    public bool isMoving = false;

    Vector2Int facingDir;

    private void Awake()
    {
        GameManager.Instance.RegisterEnemy(this);
    }
    private void Start()
    {
        Vector2Int pos = gridManager.WorldToGrid(transform.position);
        gridManager.SetOccupied(pos);
    }

    public IEnumerator StartTurnCoroutine()
    {
        Vector2Int start = gridManager.WorldToGrid(transform.position);
        gridManager.ClearOccupied(start);

        DecideState();

        switch (CurrentState)
        {
            case State.Attack:
                Attack();
                yield return new WaitForSeconds(0.2f);
                break;

            case State.Chase:
                yield return StartCoroutine(ChaseAndAttack());
                break;

            case State.Idle:
                yield return new WaitForSeconds(0.2f);
                break;
        }
        Vector2Int end = gridManager.WorldToGrid(transform.position);
        gridManager.SetOccupied(end);

        CurrentState = State.Idle;

    }

    private void DecideState()
    {
        if (enemyAttack.CanAttack(target))
            CurrentState = State.Attack;
        else
            CurrentState = State.Chase;
    }

    private IEnumerator ChaseAndAttack()
    {
        MoveByAStar();

        while (isMoving)
            yield return null;

        if (enemyAttack.CanAttack(target))
        {
            CurrentState = State.Attack;
            Attack();
            yield return new WaitForSeconds(0.2f);
        }
    }


    private void MoveByAStar()
    {

        if (isMoving) return;

        Vector2Int start = gridManager.WorldToGrid(transform.position);
        Vector2Int end = gridManager.WorldToGrid(target.position);


        List<Vector2Int> path = aStar.GetAStarPath(start, end);

        if (path == null || path.Count <= 1) return;

        int maxReach = path.Count - 2;
        int step = Mathf.Min(moveRange, maxReach);

        if (step <=0) return;

        StartCoroutine(MoveByPathCo(path, step));
    }


    private IEnumerator MoveByPathCo(List<Vector2Int> path, int steps)
    {
        isMoving = true;

        for (int i = 1; i <= steps; i++)
        {
            Vector2Int from = path[i - 1];
            Vector2Int to = path[i];
            Vector3 worldPos = gridManager.GridToWorld(to);

            // X 이동
            if (to.x != from.x)
            {
                facingDir = to.x > from.x ? Vector2Int.right : Vector2Int.left;
                yield return MoveStraightCo(
                    new Vector3(worldPos.x, transform.position.y, transform.position.z));
            }

            // Z 이동
            if (to.y != from.y)
            {
                facingDir = to.y > from.y ? Vector2Int.up : Vector2Int.down;
                yield return MoveStraightCo(
                    new Vector3(transform.position.x, transform.position.y, worldPos.z));
            }
        }

        isMoving = false;
    }

    private IEnumerator MoveStraightCo(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            Direction();
            transform.position = Vector3.MoveTowards
                (transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void Direction()
    {
        Vector3 dir = Vector3.zero;

        if (facingDir == Vector2Int.right) dir = Vector3.right;
        if (facingDir == Vector2Int.left) dir = Vector3.left;
        if (facingDir == Vector2Int.up) dir = Vector3.forward;
        if (facingDir == Vector2Int.down) dir = Vector3.back;

        if (dir == Vector3.zero) return;
        Quaternion targetRot = Quaternion.LookRotation(dir);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRot,
            360f * Time.deltaTime);
    }
    private void Attack()
    {
        enemyAttack.Attack(target);
    }
    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.UnregisterEnemy(this);
    }
}
