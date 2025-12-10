using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public enum DayType
{
    Day0,
    Day1,
    Day2
}

public enum TimePeriod
{
    Morning,
    Noon,
    Night
}

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    [Header("Current State")]
    public DayType currentDay = DayType.Day0;
    public TimePeriod currentPeriod = TimePeriod.Night;

    [Header("Settings")]
    public int maxAP = 0;
    public int actionPointsRemaining = 0;
    public GameObject restUI;

    [Header("UI")]
    public ScreenFader screenFader;

    // Events
    public event Action OnTimeAdvanced;
    public event Action OnDayAdvanced;
    public event Action OnAPChanged;

    private bool day1Started = false;
    
    //track room that entered
    private Dictionary<string, DayType> roomVisitRecord = new Dictionary<string, DayType>();

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // initialize AP
        if (currentDay == DayType.Day0)
            actionPointsRemaining = 0;  // no AP in tutorial
        else
            actionPointsRemaining = maxAP;
    }

    void Update()
    {
        if(currentDay != DayType.Day0)
        {
            if(currentPeriod == TimePeriod.Night & actionPointsRemaining == 0 & restUI != null)
            {
                restUI.SetActive(true);
            }
        }
        
    }

    // AP Consumption
    public bool TryConsumeAP(int cost)
    {
        if (!IsAPActive())
            return true;

        if (actionPointsRemaining < cost)
        {
            return false;
        }

        actionPointsRemaining -= cost;
        if (actionPointsRemaining < 0) actionPointsRemaining = 0;
        OnAPChanged?.Invoke();

        return true;
    }

    public void ConsumeAP_NoTimeAdvance(int cost)
    {
        actionPointsRemaining -= cost;
        if (actionPointsRemaining < 0)
            actionPointsRemaining = 0;

        OnAPChanged?.Invoke();
    }

    public void StartDay1()
    {
        //if (day1Started) return;
        day1Started = true;
        currentDay = DayType.Day1;
        currentPeriod = TimePeriod.Morning;

        actionPointsRemaining = maxAP;

        OnDayAdvanced?.Invoke();
        OnAPChanged?.Invoke();
    }

    public void StartNextDay()
    {
        if (currentDay == DayType.Day1)
            currentDay = DayType.Day2;
        else
        {
            Debug.Log("🏁 All Days Complete");
            SceneManager.LoadScene(2);
            return;
        }

        currentPeriod = TimePeriod.Morning;
        actionPointsRemaining = maxAP;
        roomVisitRecord.Clear();

        Debug.Log("📅 New Day Started → " + currentDay);
        restUI.SetActive(false);
        OnDayAdvanced?.Invoke();
        OnAPChanged?.Invoke();
    }

    // Time progression
    public void AdvanceTimeSlot()
    {
        StartCoroutine(AdvanceTimeRoutine());
    }

    private IEnumerator AdvanceTimeRoutine()
    {
        if (screenFader != null)
            yield return screenFader.FadeOut();

        // Advance time
        if (currentPeriod == TimePeriod.Morning)
            currentPeriod = TimePeriod.Noon;
        else if (currentPeriod == TimePeriod.Noon)
            currentPeriod = TimePeriod.Night;
        else
            currentPeriod = TimePeriod.Night;

        Debug.Log($"⏳ Time advanced → {currentPeriod}");

        OnTimeAdvanced?.Invoke();
        OnAPChanged?.Invoke();

        if (screenFader != null)
            yield return screenFader.FadeIn();
    }

    private string CurrentTimeKey()
    {
        return $"{currentDay}_{currentPeriod}";
    }

    public bool HasPaidForRoom(string roomID)
    {
        return roomVisitRecord.ContainsKey(roomID) &&
        roomVisitRecord[roomID] == currentDay;
    }

    public void MarkRoomPaid(string roomID)
    {
        if (roomVisitRecord.ContainsKey(roomID))
            roomVisitRecord[roomID] = currentDay;
        else
            roomVisitRecord.Add(roomID, currentDay);
    }

    private void ClearAllRoomPayments()
    {
        roomVisitRecord.Clear();
    }

    public bool IsAPActive()
    {
        return currentDay != DayType.Day0;
    }

    public int ActionPointsRemaining => actionPointsRemaining;

    public bool HasEnoughActionPoints(int cost)
    {
        return actionPointsRemaining >= cost;
    }
}
