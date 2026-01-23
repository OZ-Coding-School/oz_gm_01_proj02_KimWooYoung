using UnityEngine;
using UnityEngine.SceneManagement;

public class StageUIController : MonoBehaviour
{
    [SerializeField] GameObject Setteing;
    
    public void OpenSetting()
    {
        Setteing.SetActive(true);
    }
    public void CloseSetting()
    {
        Setteing.SetActive(false);
    }
    public void HomeLoad()
    {
        SceneManager.LoadScene("MainScene");
    }
}
