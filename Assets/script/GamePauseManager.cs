using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePauseManager : MonoBehaviour
{
    public static GamePauseManager Instance;

    private HashSet<string> pauseSources = new HashSet<string>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RequestPause(string source)
    {
        if (!pauseSources.Contains(source))
        {
            pauseSources.Add(source);
            UpdatePauseState();
            Debug.Log($"Paused by: {source}");
        }
    }

    public void RequestResume(string source)
    {
        if (pauseSources.Contains(source))
        {
            pauseSources.Remove(source);
            UpdatePauseState();
            Debug.Log($"Resumed by: {source}");
        }
    }

    private void UpdatePauseState()
    {
        Time.timeScale = (pauseSources.Count > 0) ? 0f : 1f;
    }

    public bool IsPaused => pauseSources.Count > 0;
}
