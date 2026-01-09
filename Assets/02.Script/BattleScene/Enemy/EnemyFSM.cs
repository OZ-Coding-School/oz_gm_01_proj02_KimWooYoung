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
    public float chaseDistance = 100f;
    public float attackDistance = 3f;
    public float speed = 2.0f;

    private void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                break;
                case State.Chase:
                Rotate();
                break;
                case State.Attack:
                Rotate();
                break;
        }
        TransitionCheck();
    }

    private void Rotate()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        Vector3 direction = (target.position - transform.position).normalized;

        direction.y = 0;

        if (direction == Vector3.zero) return;

        transform.forward = direction;

    }

    private void TransitionCheck()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance < attackDistance)
        {
            currentState = State.Attack;
        }
        else if (distance < chaseDistance)
        {
            currentState = State.Chase;

        }
        else
        {
            currentState = State.Idle;
        }
    }
}
