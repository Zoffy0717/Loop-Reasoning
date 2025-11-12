using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool isPaused;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        GamePauseManager.Instance.RequestPause("Menu");
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        GamePauseManager.Instance.RequestResume("Menu");
        isPaused = false;
    }

    public void OnResumeButtonClicked()
    {
        ResumeGame();
    }

    public void OnSettingsButtonClicked()
    {
        PauseGame();
    }
}
