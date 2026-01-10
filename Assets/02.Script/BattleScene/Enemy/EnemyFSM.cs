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
    public Transform target;
    [SerializeField]private GridManager gridManager;
    [SerializeField]private AStarPathfinder aStar;

    private int moveRange = 3;

    Vector2Int facingDir;


    public bool isMoving = false;

    private void Update()
    {
        CheckTest();
        ChangeState();

    }
    private void CheckTest()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            currentState = State.Chase;
        }
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
        int step = Mathf.Min(moveRange, path.Count -1);

        StartCoroutine(MoveByPath(path, step));
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

    IEnumerator MoveByPath(List<Vector2Int>path, int steps)
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
            yield return MoveStraight(xTarget);

            if (to.y > from.y) facingDir = Vector2Int.up;
            else if (to.y < from.y) facingDir = Vector2Int.down;

            Vector3 zTarget = new Vector3
                (transform.position.x, tileWoldPos.y, tileWoldPos.z);
            yield return MoveStraight(zTarget);
        }
            isMoving = false;
            currentState = State.Idle;
    }

    IEnumerator MoveStraight(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            Direction();
            transform.position = Vector3.MoveTowards
                (transform.position, target, 3f * Time.deltaTime);
            yield return null;
        }
    }

}
