using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour, IInteractable
{

    public DialogueData dialogueData;
    public CardSY npcCard;
    public GameObject interactHintUI;
    private bool cardDroped = false;


    public void Interact(PlayerInteraction player)
    {
        var router = GetComponent<NPCDialogueRouter>();

        // -------------------------
        // 1. NPC uses dialogue router
        // -------------------------
        if (router != null)
        {
            // Step A: If dialogue is NOT active, start it
            if (!DialogueUI.Instance.IsActive())
            {
                router.Interact(player); // starts the router-based dialogue
                return;
            }

            // Step B: Advance router-based dialogue
            bool stillTalking = DialogueUI.Instance.AdvanceDialogue();

            // Step C: When dialogue ends → drop card ONCE
            if (!stillTalking && !cardDroped)
            {
                player.cardInventory.AddCard(npcCard);
                UI_CardPopup.Instance.ShowCards(new[] { npcCard });
                cardDroped = true;
            }

            return;
        }

        // -------------------------
        // 2. OLD (non-router) NPC logic
        // -------------------------
        if (!DialogueUI.Instance.IsActive())
        {
            DialogueUI.Instance.StartDialogue(dialogueData);
            return;
        }
        else
        {
            bool stillTalking = DialogueUI.Instance.AdvanceDialogue();

            if (!stillTalking && !cardDroped)
            {
                player.cardInventory.AddCard(npcCard);
                UI_CardPopup.Instance.ShowCards(new[] { npcCard });
                cardDroped = true;
            }
        }
    }

}
