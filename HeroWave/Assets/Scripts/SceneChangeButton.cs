using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeButton : MonoBehaviour
{
    [SerializeField] private string targetSceneName;

    public void ChangeScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(targetSceneName);
    }
}
