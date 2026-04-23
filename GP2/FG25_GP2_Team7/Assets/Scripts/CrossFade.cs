using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrossFade : MonoBehaviour
{
    private Animator Animator;
    private GameObject MainMenuMusicAudio;
    public int PublicLevelIndex;

    void Start()
    {
        Animator = GetComponent<Animator>();
    }

    public void LevelTrans(int LevelIndex)
    {
        if(LevelIndex == 9999)
        {
            for (int i = 1; i < SceneManager.sceneCountInBuildSettings - 1; i++)
            {
                LevelIndex = i;
                if (PlayerPrefs.GetInt(i.ToString(), 0) < 1)
                {
                    break;
                }
                if (i == SceneManager.sceneCountInBuildSettings - 2)
                    LevelIndex = 1;
            }
        }
        
        
        StartCoroutine(Joesph(LevelIndex));
    }

    public IEnumerator Joesph(int LevelIndex)
    {
        Time.timeScale = 1.0f;
        Animator.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        MainMenuMusicAudio = GameObject.FindWithTag("MainMenuMusic");
        if (LevelIndex == 1)
        {
            SceneManager.LoadSceneAsync(15);
        }
        else
        {
            Destroy(MainMenuMusicAudio);
            SceneManager.LoadSceneAsync(LevelIndex);
        }
            
    }
}
