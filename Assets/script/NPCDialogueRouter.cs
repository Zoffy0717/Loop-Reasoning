using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogueRouter : MonoBehaviour, IInteractable
{
    public NPCDialogueSet dialogueSet;     // ← Assign in Inspector

    public void Interact(PlayerInteraction player)
    {
        var gsm = GameStateManager.Instance;
        if (gsm == null)
        {
            Debug.LogError("No GameStateManager found.");
            return;
        }

        // Get current dialogue based on day + time
        DialogueData data = dialogueSet.GetDialogue(
            gsm.currentDay,
            gsm.currentPeriod
        );

        if (data == null)
        {
            Debug.Log("NPC has no dialogue for this time slot.");
            return;
        }

        // Open your dialogue system normally
        DialogueUI.Instance.StartDialogue(data);
    }
}

