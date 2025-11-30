using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NPC/NPC Dialogue Schedule")]
public class NPCDialogueSet : ScriptableObject
{
    [System.Serializable]
    public class DialogueByTime
    {
        public DayType day;                  // Day1 / Day2
        public TimePeriod time;              // Morning / Noon / Night
        public DialogueData dialogue;        // <- Your existing dialogue SO
    }

    public DialogueByTime[] scheduledDialogues;

    public DialogueData GetDialogue(DayType currentDay, TimePeriod currentTime)
    {
        foreach (var entry in scheduledDialogues)
        {
            if (entry.day == currentDay && entry.time == currentTime)
                return entry.dialogue;
        }

        Debug.LogWarning("No dialogue found for current schedule!");
        return null;
    }
}
