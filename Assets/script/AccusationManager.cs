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
        // Find CardInventory across scenes (persistent manager)
        cardInventory = FindObjectOfType<CardInventory>();

        board.SetActive(true);

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
        restartPanel.SetActive(true);
        yield return new WaitForSeconds(introDuration);
        SceneManager.LoadScene(1);
        GameStateManager.Instance.StartDay1();
        
    }
}
