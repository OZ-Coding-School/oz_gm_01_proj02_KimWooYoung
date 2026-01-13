using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    public enum State
    {
        Idle,
        Chase,
        Attack
    }

    public State currentState = State.Idle;
    [SerializeField] private Transform target;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private AStarPathfinder aStar;
    [SerializeField] private BaseSO baseSO;

    private int moveRange = 3;
    private float moveSpeed = 3f;

    public bool IsMoving => isMoving;

    Vector2Int facingDir;


    public bool isMoving = false;

    public void StartTrun()
    {

        StartCoroutine(EnemyTurnRoutine());
        currentState = State.Chase;
        ChangeState();
    }

    private void ChangeState()
    {
        switch (currentState)
        {
            case State.Idle:
                break;
            case State.Chase:
                MoveByAStar();
                break;
            case State.Attack:
                break;
        }
    }

    private void MoveByAStar()
    {

        if (isMoving) return;

        Vector2Int start = gridManager.WorldToGrid(transform.position);
        Vector2Int end = gridManager.WorldToGrid(target.position);


        List<Vector2Int> path = aStar.GetAStarPath(start, end);

        if (path == null || path.Count <= 1) return;

        //이동량 만큼 이동
        int maxReach = path.Count - 2;
        int step = Mathf.Min(moveRange, maxReach);

        if (step <=0)
        {
            return;
        }

        StartCoroutine(MoveByPathCo(path, step));
    }

    private IEnumerator MoveByPathCo(List<Vector2Int>path, int steps)
    {
        isMoving = true;

        for (int i = 1; i <= steps; i++)
        {
            Vector2Int from = path[i - 1];
            Vector2Int to = path[i];

            Vector3 tileWoldPos = gridManager.GridToWorld(to);

            //현재 방향
            if (to.x > from.x) facingDir = Vector2Int.right;
            else if (to.x < from.x) facingDir = Vector2Int.left;

            Vector3 xTarget = new Vector3
                (tileWoldPos.x, tileWoldPos.y, transform.position.z);
            yield return MoveStraightCo(xTarget);

            if (to.y > from.y) facingDir = Vector2Int.up;
            else if (to.y < from.y) facingDir = Vector2Int.down;

            Vector3 zTarget = new Vector3
                (transform.position.x, tileWoldPos.y, tileWoldPos.z);
            yield return MoveStraightCo(zTarget);
        }
        isMoving = false;
        currentState = State.Idle;
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

    private IEnumerator EnemyTurnRoutine()
    {
        if (CanAttackTarget())
        {
            isMoving = true;
            Attack();
            yield return null;
            isMoving = false; 
            yield break;
        }
        MoveByAStar();

        while (isMoving) yield return null;

        if (CanAttackTarget())
        {
            Attack();
        }
    }

    private bool CanAttackTarget()
    {
        Vector2Int enemyPos = gridManager.WorldToGrid(transform.position);
        Vector2Int targetPos = gridManager.WorldToGrid(target.position);

        int dist = Mathf.Abs(enemyPos.x - targetPos.x) + Mathf.Abs(enemyPos.y - targetPos.y);

        return dist <= baseSO.attackRange;
    }

    private void Attack()
    {
        Debug.Log("[Enemy] Attack!");

        Health targetHealth = target.GetComponent<Health>();
        if (targetHealth == null) return;

        targetHealth.TakeDamage(baseSO.Damage);
    }

}
