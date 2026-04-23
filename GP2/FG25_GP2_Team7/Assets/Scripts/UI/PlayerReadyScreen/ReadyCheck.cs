using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class ReadyCheck : MonoBehaviour
{
    InputAction JumpAction;
    [SerializeField] GameObject Player1NotReady;
    [SerializeField] GameObject Player1Ready;
    [SerializeField] GameObject Player2NotReady;
    [SerializeField] GameObject Player2Ready;
    //public CrossFade cs; // for crossfade

    bool Player1 = false;
    bool Player2 = false;
    private GameObject MainMenuMusicAudio;
    public Animator Animator;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player1Ready.SetActive(Player1);
        Player2Ready.SetActive(Player2);
        JumpAction = InputSystem.actions.FindAction("Jump");
    }

    // Update is called once per frame
    
    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Player1 = !Player1;
            Player1Ready.SetActive(Player1);
            Player1NotReady.SetActive(!Player1);
        }
        if (JumpAction.WasPressedThisFrame())
        {
            Player2 = !Player2;
            Player2Ready.SetActive(Player2);
            Player2NotReady.SetActive(!Player2);
        }
        if (Player1 && Player2)
        {
            StartCoroutine(Crossfade());
        }
    }
    private IEnumerator Crossfade()
    {
        Time.timeScale = 1.0f;
        Animator.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        MainMenuMusicAudio = GameObject.FindWithTag("MainMenuMusic");
        Destroy(MainMenuMusicAudio);
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
