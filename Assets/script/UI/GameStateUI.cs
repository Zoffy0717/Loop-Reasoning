using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameStateUI : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI dayValueText;
    public TextMeshProUGUI apValueText;
    public TextMeshProUGUI timeValueText;

    private GameStateManager gsm;

    private void Start()
    {
        gsm = GameStateManager.Instance;

        if (gsm == null)
        {
            Debug.LogError("❌ GameStateManager not found!");
            return;
        }

        // Subscribe to GameStateManager events
        gsm.OnAPChanged += UpdateUI;
        gsm.OnDayAdvanced += UpdateUI;
        gsm.OnTimeAdvanced += UpdateUI;

        // Initialize display
        UpdateUI();
    }

    private void OnDestroy()
    {
        if (gsm != null)
        {
            gsm.OnAPChanged -= UpdateUI;
            gsm.OnDayAdvanced -= UpdateUI;
            gsm.OnTimeAdvanced -= UpdateUI;
        }
    }

    private void UpdateUI()
    {
        if (gsm == null) return;

        if (dayValueText != null)
            dayValueText.text = gsm.currentDay.ToString();

        if (apValueText != null)
            apValueText.text = gsm.actionPointsRemaining.ToString();

        if (timeValueText != null)
            timeValueText.text = gsm.currentPeriod.ToString();
    }
}