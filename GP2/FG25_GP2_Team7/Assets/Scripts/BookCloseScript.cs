using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class BookCloseScript : MonoBehaviour
{
    public Animator BookCover;
    public Animator Page;
    public Animator BookMove;
    public Animator Animator;
    void Start()
    {
        StartCoroutine(BookCloseCouroutine());
    }

    IEnumerator BookCloseCouroutine()
    {
        print("Couroutine Started");
        yield return new WaitForSeconds(6f);
        BookCover.SetTrigger("CLOSENOW");
        yield return new WaitForSeconds(0.05f);
        Page.SetTrigger("CLOSENOW");
        print("Couroutine Ended");
        yield return new WaitForSeconds(0.80f);
        BookMove.SetTrigger("MOVENOW");
        yield return new WaitForSeconds(4f);
        Time.timeScale = 1.0f;
        Animator.SetTrigger("Start");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadSceneAsync(0);
    }

}
