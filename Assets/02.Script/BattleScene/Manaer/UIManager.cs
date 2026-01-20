using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private GameObject skillSelcet;
    [SerializeField] private GameObject playerTurnPanel;
    [SerializeField] private GameObject gameClear;
    [SerializeField] private GameObject gameOver;

    [SerializeField] private TurnManager turnManager;

    public void AttackUIOpen()
    {
        playerTurnPanel.SetActive(true);
    }
    public void AttackUIClose()
    {
        playerTurnPanel.SetActive(false);
    }

    public void OnAttackButton()
    {
        playerAttack.ShowAttackRange(false);
        playerTurnPanel.SetActive(false);
    }
    public void OnSkillButton()
    {
        skillSelcet.SetActive(true);
        playerTurnPanel.SetActive(false);
    }
    public void SkillPanelClose()
    {
        skillSelcet.SetActive(false);
    }
    public void TunrOffButton()
    {
        turnManager.EndPlayerTunr();
        playerTurnPanel.SetActive(false);
    }
    public void GameCler()
    {
        gameClear.SetActive(true);
    }
    public void GameOver()
    {
        gameOver.SetActive(true);

    }
    public void OkButton()
    {
        Application.Quit();

    }
}
