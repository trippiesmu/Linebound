using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ReadyCheck : MonoBehaviour
{
    InputAction JumpAction;
    [SerializeField] GameObject Player1NotReady;
    [SerializeField] GameObject Player1Ready;
    [SerializeField] GameObject Player2NotReady;
    [SerializeField] GameObject Player2Ready;

    bool Player1 = false;
    bool Player2 = false;

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
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
    }
}
