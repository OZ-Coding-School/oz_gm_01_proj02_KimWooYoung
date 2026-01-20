using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private PlayerUnit playerUnit;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private GridHighlight gridHighlight;
    [SerializeField] private UIManager uiManager;

    private SkillSO currentSkill;

    private List<Vector2Int> attackRange = new List<Vector2Int>();
    private List<Health> healthUnit = new List<Health>();
    private List<Vector2Int> areaPreview = new List<Vector2Int>();


    private bool isSelectingAttack = false;
    private bool isSkillAttack = false;
    public bool HasConfirmedAttack { get; private set; }

    private void Awake()
    {
        healthUnit.AddRange(FindObjectsOfType<Health>());
    }
    public void ShowNormalAttackRange()
    {
        currentSkill = null;
        ShowAttackRange(false);
    }
    public void ShowAttackRange(bool useSkill)
    {
        isSkillAttack = useSkill;

        Vector2Int center = gridManager.WorldToGrid(transform.position);

        int range;

        if (isSkillAttack)
        {
            if (currentSkill == null)
            {
                return;
            }
            range = currentSkill.range;
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


    private List<Vector2Int> BuildAreaRange(Vector2Int center, int size)
    {
        List<Vector2Int> result = new();

        for (int x = -size; x <= size; x++)
        {
            for (int y = -size; y <= size; y++)
            {
                if (Mathf.Abs(x) + Mathf.Abs(y) > size)
                    continue;

                result.Add(center + new Vector2Int(x, y));
            }
        }

        return result;
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

        if (isSkillAttack && currentSkill != null && currentSkill.type == TargetType.Area)
        {
            PreviewArea();
        }


        if (Input.GetMouseButtonDown(0))
        {
            TrySkill();
        }
    }
    private void TrySkill()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit)) return;

        Vector2Int clickGrid = gridManager.WorldToGrid(hit.point);

        if (!attackRange.Contains(clickGrid)) return;

        HasConfirmedAttack = true;
        
        if (isSkillAttack)
        {
            ExecuteSkillByType(clickGrid);
        }
        else
        {
            ExecuteNormalAttack(clickGrid);
        }

        EndAttack();

    }
    private void ExecuteNormalAttack(Vector2Int clickGrid)
    {
        foreach (var hp in healthUnit)
        {
            if (hp == null) continue;      
            if (hp.isDead) continue;

            Vector2Int hpGrid = gridManager.WorldToGrid(hp.transform.position);

            if (hpGrid == clickGrid)
            {
                hp.TakeDamage(playerUnit.Data.Damage);
                return;
            }
        }
    }

    private void ExecuteSkillByType(Vector2Int clickGrid)
    {
        if (currentSkill == null) return;

        switch (currentSkill.type)
        {
            case TargetType.Single:
                SingleAttack(clickGrid);
                break;

            case TargetType.Area:
                AreaAttack(clickGrid);
                break;

            case TargetType.Heal:
                SingleHeal(clickGrid);
                break;
        }
    }
    private void SingleAttack(Vector2Int targetGrid)
    {
        foreach (var hp in healthUnit)
        {
            if (hp.isDead) continue;

            Vector2Int hpGrid = gridManager.WorldToGrid(hp.transform.position);

            if (hpGrid == targetGrid)
            {
                hp.TakeDamage(currentSkill.damage);
                return;
            }
        }
    }

    private void AreaAttack(Vector2Int targetCenter)
    {
        foreach (var hp in healthUnit)
        {
            if (hp.isDead) continue;

            Vector2Int hpGrid = gridManager.WorldToGrid(hp.transform.position);

            int dist = Mathf.Abs(hpGrid.x - targetCenter.x) +
                Mathf.Abs(hpGrid.y - targetCenter.y);

            if (dist <= currentSkill.areaSize)
            {
                hp.TakeDamage(currentSkill.damage);
            }
        }
    }

    private void SingleHeal(Vector2Int targetGrid)
    {
        foreach (var hp in healthUnit)
        {
            Vector2Int hpGrid = gridManager.WorldToGrid(hp.transform.position);

            if (hpGrid == targetGrid)
            {
                hp.Heal(currentSkill.damage);
                return;
            }
        }
    }

    private void PreviewArea()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit)) return;

        Vector2Int grid = gridManager.WorldToGrid(hit.point);

        // 사거리 밖이면 미리보기 안함
        if (!attackRange.Contains(grid)) return;

        // 기존 하이라이트 초기화
        gridManager.ResetAllTiles(Color.white);

        // 사거리 다시 표시
        gridManager.HighlightTiles(attackRange, Color.red);

        // Area 범위 표시
        areaPreview = BuildAreaRange(grid, currentSkill.areaSize);
        gridManager.HighlightTiles(areaPreview, new Color(1f, 0.5f, 0f)); // 주황색
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
        isSkillAttack = false;
        currentSkill = null;
    }

    public void RestTurn()
    {
        HasConfirmedAttack = false;
    }
    public void SetSkill(SkillSO skill)
    {
        currentSkill = skill;
    }
}
