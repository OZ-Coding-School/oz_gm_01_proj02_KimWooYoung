using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private PlayerUnit playerUnit;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private GridHighlight gridHighlight;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private SkillSO skillSO;

    private List<Vector2Int> attackRange = new List<Vector2Int>();

    private bool isSelectingAttack = false;
    private bool isSkillAttack = false;
    public bool HasConfirmedAttack { get; private set; }
    public bool isAttacking { get; private set; }


    public void ShowAttackRange(bool useSkill)
    {
       isSkillAttack = useSkill;

        Vector2Int center = gridManager.WorldToGrid(transform.position);

        int range;

        if (isSkillAttack)
        {
            range = skillSO.range;
        }
        else
        {
            range = playerUnit.Data.attackRange;
        }

        BuildAttackRange(center, range);
        gridManager.HighlightTiles(attackRange, Color.red);

        HasConfirmedAttack = false;
        isSelectingAttack = true;
    }


    private void BuildAttackRange(Vector2Int center, int range)
    {
        attackRange.Clear();

        for (int x = -range; x <= range; x++)
        {
            for (int y = -range; y <= range; y++)
            {
                if (Mathf.Abs(x) + Mathf.Abs(y) > range)
                    continue;

                attackRange.Add(center + new Vector2Int(x, y));
            }
        }
    }
    private void Update()
    {
        if (!isSelectingAttack)
        {
            return;
        }

        if (Input.GetMouseButtonDown(1))
        {
            CancelAttack();
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            TryAttack();
        }
    }

    private void TryAttack()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit)) return;
       
        Vector2Int clickGrid = gridManager.WorldToGrid(hit.point);

        if (!attackRange.Contains(clickGrid)) return;
        

        HasConfirmedAttack = true;

        Health[] allHealths = FindObjectsOfType<Health>();

        foreach (var hp in allHealths)
        {
            if (hp.isDead) continue;

            Vector2Int hpGrid = gridManager.WorldToGrid(hp.transform.position);

            if (hpGrid == clickGrid)
            {
                ExecuteAttack(hp);
                return;
            }
        }

        EndAttack();
    }

    private void ExecuteAttack(Health target)
    {
        int damage;

        if (isSkillAttack)
        {
            damage = skillSO.damage;
        }
        else
        {
            damage = playerUnit.Data.Damage;
        }

        target.TakeDamage(damage);

        gridManager.ResetAllTiles(Color.white);
        isSelectingAttack = false;
        HasConfirmedAttack = true;
            isSkillAttack = false;

    }

    private void CancelAttack()
    {
        isSelectingAttack = false;
        attackRange.Clear();
        gridManager.ResetAllTiles(Color.white);
        uiManager.AttackUIOpen();
    }

    private void EndAttack()
    {
        gridManager.ResetAllTiles(Color.white);
        isSelectingAttack = false;
    }

    public void RestTurn()
    {
        HasConfirmedAttack = false;
    }
}
