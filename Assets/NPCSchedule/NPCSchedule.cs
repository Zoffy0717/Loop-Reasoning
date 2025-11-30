using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NPC/NPC Schedule")]
public class NPCSchedule : ScriptableObject
{
    [Serializable]
    public class ScheduleEntry
    {
        public int anchorIndex = 0;
        public string roomID;         // "Kitchen", "Hallway", "Library"
        public int chapter;           // which chapter or story stage
        public string timeSlot;       // "Morning", "Afternoon", "Night"
        public Vector2 spawnPosition; // where to spawn (inside the room)
    }

    public string npcID;
    public GameObject npcPrefab;

    public List<ScheduleEntry> schedule = new List<ScheduleEntry>();
}