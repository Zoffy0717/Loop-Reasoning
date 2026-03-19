using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AccusationManager : MonoBehaviour
{
    [Header("Suspects")]
    public AccusationCharacterButton[] suspects;

    [Header("Game Logic")]
    public string correctSuspectID;

    [Header("UI")]
    public GameObject gameOverPanel;
    public GameObject board;
    public GameObject restartPanel;

    private AccusationCharacterButton selectedSuspect;

    private CardInventory cardInventory;

    private float introDuration = 2f;
    void Start()
    {
        cardInventory = FindObjectOfType<CardInventory>();

        board.SetActive(true);

        int availableChoices = 0;

        foreach (var s in suspects)
        {
            bool hasEvidence =
                cardInventory != null &&
                cardInventory.HasAnyCard(s.requiredEvidenceCardIDs);

            if (hasEvidence)
            {
                availableChoices++;
            }

            s.Init(this, cardInventory);
        }

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (availableChoices == 0)
        {
            StartCoroutine(RestartGame());
        }
    }

    public void SelectSuspect(AccusationCharacterButton suspect)
    {
        if (selectedSuspect != null)
            selectedSuspect.SetSelected(false);

        selectedSuspect = suspect;
        selectedSuspect.SetSelected(true);
    }

    public void ConfirmAccusation()
    {
        if (selectedSuspect == null)
        {
            Debug.Log("No suspect selected.");
            return;
        }

        if (selectedSuspect.suspectID == correctSuspectID)
        {
            Debug.Log("✅ Correct accusation!");
            ShowGameOver();
        }
        else
        {
            Debug.Log("❌ Wrong accusation. Restarting Day 1.");
            StartCoroutine(RestartGame());
        }
    }

    private void ShowGameOver()
    {
        board.SetActive(false);
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    private IEnumerator RestartGame()
    {
        board.SetActive(false);
        restartPanel.SetActive(true);
        yield return new WaitForSeconds(introDuration);
        SceneManager.LoadScene(1);
        GameStateManager.Instance.StartDay1();
        
    }

    public void Back()
    {
        SceneManager.LoadScene(0);
    }

    public void CompleteRestart()
    {
        Destroy(GameObject.Find("GM"));
        Destroy(GameObject.Find("AudioManager"));
        Destroy(GameObject.Find("FadeCanvas"));
        SceneManager.LoadScene(1);
    }
}
