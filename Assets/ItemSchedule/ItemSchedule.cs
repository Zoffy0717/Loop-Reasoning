using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item Schedule")]
public class ItemSchedule : ScriptableObject
{
    [Serializable]
    public class ItemEntry
    {
        public string roomID;          // "Kitchen", "Garage", "003"
        public DayType day;            // Day0, Day1, Day2
        public TimePeriod period;    // Morning / Noon / Night
        public int anchorIndex = 0;
    }

    public string itemID;
    public GameObject itemPrefab;

    public ItemEntry[] schedule;

    public bool pickedUp = false;
}