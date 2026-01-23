using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainUIController : MonoBehaviour
{
    [SerializeField] GameObject loadPanel;
    [SerializeField] GameObject savePanel;

    public Button continueButton;
    public Button[] SaveSlotButton = new Button[4];
    public Button[] loadSlotButton = new Button[4];

    private void Start()
    {
        if (DataManager.Instance != null)
        {
            UpdateContinueButton();
        }
    }

    public void OnStartNew()
    {
        DataManager.Instance.SetCurrentSlot(1); 
        SceneManager.LoadScene("StageSelectScene");
    }

    public void OnSelectSaveSlot(int slotIndex)
    {
        DataManager.Instance.SetCurrentSlot(slotIndex);
        DataManager.Instance.SaveCurrentSlot();

        Debug.Log("저장 경로: " + Application.persistentDataPath);

        UpdateContinueButton();
        UpdateLoadSlotButtons();

        savePanel.SetActive(false); // 저장 후 닫기 (선택)


    }
    private void UpdateSaveSlotButtons()
    {
        for (int i = 1; i <= 4; i++)
        {
            bool hasSave = DataManager.Instance.HasSaveSlot(i);

            // interactable은 항상 true (덮어쓰기 가능)
            SaveSlotButton[i - 1].interactable = true;

        }
    }

    public void OnContinue()
    {
        int lastSlot = PlayerPrefs.GetInt("LastSaveSlot", 1);

        if (DataManager.Instance.HasSaveSlot(lastSlot))
        {
            DataManager.Instance.SetCurrentSlot(lastSlot);
            DataManager.Instance.LoadAllData(lastSlot);
            SceneManager.LoadScene("StageSelectScene");
        }
        else
        {
            Debug.Log("저장된 세이브가 없습니다!");
        }
    }

    public void OnLoadSlot(int slotIndex)
    {
        if (DataManager.Instance.HasSaveSlot(slotIndex))
        {
            DataManager.Instance.SetCurrentSlot(slotIndex);
            DataManager.Instance.LoadAllData(slotIndex);
            SceneManager.LoadScene("StageSelectScene");
        }
        else
        {
            Debug.Log($"슬롯 {slotIndex}에 저장된 데이터가 없습니다!");
        }
    }
    private void UpdateContinueButton()
    {
        bool anySave = false;

        for (int i = 1; i <= 4; i++)
        {
            if (DataManager.Instance.HasSaveSlot(i))
            {
                anySave = true;
                break;
            }
        }

        continueButton.interactable = anySave;
    }
    public void ShowSave()
    {
        savePanel.SetActive(true);
        UpdateSaveSlotButtons();

    }
    public void CloseSave()
    {
        savePanel.SetActive(false);
    }
    public void ShowLoad()
    {
        loadPanel.SetActive(true);
        UpdateLoadSlotButtons();
    }
    public void CloseLoad()
    {
        loadPanel.SetActive(false);
    }
    private void UpdateLoadSlotButtons()
    {
        for (int i = 1; i <= 4; i++)
        {
            loadSlotButton[i - 1].interactable =
                DataManager.Instance.HasSaveSlot(i);
        }
    }
    public void QuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void DevResetAllData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Debug.Log(" 모든 세이브 데이터 초기화 완료");
    }

}
