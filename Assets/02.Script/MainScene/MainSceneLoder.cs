using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneLoder : MonoBehaviour
{
    public void StageSelectSceneLoder()
    {
        SceneManager.LoadScene("StageSelectScene");
    }
}
