using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    void Start()
    {

    }

    public void travel()
    {
        Debug.Log(SceneManager.GetActiveScene().buildIndex);
        Debug.Log(SceneManager.sceneCountInBuildSettings);
        if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings-1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
            Time.timeScale = 1;
        }
    }
}
