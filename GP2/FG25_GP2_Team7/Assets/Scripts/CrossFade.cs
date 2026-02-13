using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrossFade : MonoBehaviour
{
    private Animator Animator;

    void Start()
    {
        Animator = GetComponent<Animator>();
    }

    public void LevelTrans(int LevelIndex)
    {
        StartCoroutine(Joesph(LevelIndex));
    }

    public IEnumerator Joesph(int LevelIndex)
    {
        Time.timeScale = 1.0f;
        Animator.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadSceneAsync(LevelIndex);
    }
}
