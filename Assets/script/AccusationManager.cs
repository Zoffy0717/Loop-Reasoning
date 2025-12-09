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

    private AccusationCharacterButton selectedSuspect;

    private CardInventory cardInventory;

    void Start()
    {
        // Find CardInventory across scenes (persistent manager)
        cardInventory = FindObjectOfType<CardInventory>();

        foreach (var s in suspects)
        {
            bool hasEvidence =
                cardInventory != null &&
                cardInventory.HasCard(s.requiredEvidenceCardID);

            s.Init(this, hasEvidence);
        }

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
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
            RestartGame();
        }
    }

    private void ShowGameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    private void RestartGame()
    {
        GameStateManager.Instance.StartDay1();
        SceneManager.LoadScene(1);
    }
}
