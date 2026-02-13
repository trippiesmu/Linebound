using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [TextArea(5, 1000)]
    public string Comment = "This uses the way kevin does buttons so just assign the Resume/ReturnToMainMenu/Quit to their own buttons in the buttons see example for refference.";

    bool Paused;
    InputAction MenuAction;

    List<Transform> children = new List<Transform>();
    
    private void Start()
    {
        MenuAction = InputSystem.actions.FindAction("Menu");

        foreach(Transform child in GetComponentsInChildren<Transform>())
        {
            children.Add(child);
        }
        children.RemoveAt(0);

        HideHUD();
    }


    private void Update()
    {
        if (MenuAction.WasPressedThisFrame() 
            && ((Paused && Time.timeScale == 0) 
                || (!Paused && Time.timeScale == 1)))
        {
            try
            {
                if (Time.timeScale != 0 && !children[0].gameObject.activeSelf)
                {
                    ShowHUD();
                }
                else
                {
                    Time.timeScale = 1;

                    Resume();
                }
            }
            catch (Exception e)
            {

            }
            
        }
    }



    public void Resume()
    {
        HideHUD();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu2BOOK");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void ShowHUD()
    {
        Paused = true;
        Time.timeScale = 0;
        foreach (Transform child in children)
        {
            child.gameObject.SetActive(true);
        }

    }

    void HideHUD()
    {
        Time.timeScale = 1;
        Paused = false;
        foreach (Transform child in children)
        {
            child.gameObject.SetActive(false);
        }
        Time.timeScale = 1;

    }
}
