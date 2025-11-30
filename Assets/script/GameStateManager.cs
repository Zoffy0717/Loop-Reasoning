using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;

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

    [Header("UI")]
    public ScreenFader screenFader;

    // Events
    public event Action OnTimeAdvanced;
    public event Action OnDayAdvanced;
    public event Action OnAPChanged;

    private bool day1Started = false;
    //track room that entered
    private Dictionary<string, TimePeriod> roomVisitRecord = new Dictionary<string, TimePeriod>();

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

    // AP Consumption
    public bool TryConsumeAP(int cost)
    {
        // AP inactive → always allow free entry
        if (!IsAPActive())
            return true;

        if (actionPointsRemaining < cost)
        {
            Debug.Log("❌ Not enough AP.");
            return false;
        }

        actionPointsRemaining -= cost;

        OnAPChanged?.Invoke();

        AdvanceTimeSlot();

        return true;
    }

    public void StartDay1()
    {
        if (day1Started) return;
        day1Started = true;
        currentDay = DayType.Day1;
        currentPeriod = TimePeriod.Morning;

        actionPointsRemaining = maxAP;

        Debug.Log("🌅 DAY 1 BEGINS — AP System Activated");

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

    // Day progression
    private void AdvanceDay()
    {
        if (currentDay == DayType.Day1)
        {
            currentDay = DayType.Day2;
            currentPeriod = TimePeriod.Morning;

            actionPointsRemaining = maxAP;

            Debug.Log("📅 Advanced to Day 2");
            OnDayAdvanced?.Invoke();
            OnAPChanged?.Invoke();
        }
        else
        {
            Debug.Log("⭐ GAME END: Days Completed");
            // TODO: end game
        }
    }

    public void StartNextDay()
    {
        if (currentDay == DayType.Day1)
            currentDay = DayType.Day2;
        else
        {
            Debug.Log("🏁 All Days Complete");
            // End game
            return;
        }

        currentPeriod = TimePeriod.Morning;
        actionPointsRemaining = maxAP;

        Debug.Log("📅 New Day Started → " + currentDay);

        OnDayAdvanced?.Invoke();
        OnAPChanged?.Invoke();
    }

    // Helpers
    public bool IsFreeRoom()
    {
        return (currentDay == DayType.Day1 &&
                currentPeriod == TimePeriod.Morning);
    }

    public string GetCurrentStateString()
    {
        return $"{currentDay} - {currentPeriod} - AP:{actionPointsRemaining}";
    }
    public bool HasEnoughActionPoints(int cost)
    {
        return actionPointsRemaining >= cost;
    }

    public void UseActionPoints(int cost)
    {
        TryConsumeAP(cost); // reuse existing logic
    }

    // Optional event hook (room-based spawning can use this)
    public void NotifyRoomEntered(string roomID)
    {
        Debug.Log("📍 Room entered: " + roomID);
        // Later you can add:
        //NPCScheduleManager.Instance.UpdateNPCPositions(roomID);
        // ItemSpawnManager.Instance.RefreshRoom(roomID);
    }

    public bool HasPaidForRoom(string roomID)
    {
        if (roomVisitRecord.ContainsKey(roomID))
        {
            return roomVisitRecord[roomID] == currentPeriod;
        }
        return false;
    }

    public void MarkRoomPaid(string roomID)
    {
        if (roomVisitRecord.ContainsKey(roomID))
            roomVisitRecord[roomID] = currentPeriod;
        else
            roomVisitRecord.Add(roomID, currentPeriod);
    }

    // Property wrapper so RoomEntrance can read AP
    public int ActionPointsRemaining
    {
        get { return actionPointsRemaining; }
    }

    public bool IsAPActive()
    {
        return currentDay != DayType.Day0;
    }
}
