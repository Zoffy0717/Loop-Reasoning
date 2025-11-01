using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePauseManager : MonoBehaviour
{
    public static GamePauseManager Instance { get; private set; }

    private int pauseRequests = 0; // how many systems want the game paused

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void RequestPause()
    {
        pauseRequests++;
        UpdatePauseState();
    }

    public void RequestResume()
    {
        pauseRequests = Mathf.Max(0, pauseRequests - 1);
        UpdatePauseState();
    }

    private void UpdatePauseState()
    {
        Time.timeScale = (pauseRequests > 0) ? 0f : 1f;
    }

    public bool IsPaused() => pauseRequests > 0;
}
