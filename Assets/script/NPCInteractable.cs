using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour, IInteractable
{

    public DialogueData dialogueData;
    public CardSY npcCard;
    public GameObject interactHintUI;


    public void Interact(PlayerInteraction player)
    {
        var router = GetComponent<NPCDialogueRouter>();

        // -------------------------
        // 1. NPC uses dialogue router
        // -------------------------
        if (router != null)
        {
            //If dialogue is NOT active, start it
            if (!DialogueUI.Instance.IsActive())
            {
                router.Interact(player); // starts the router-based dialogue
                return;
            }

            // Advance router-based dialogue
            bool stillTalking = DialogueUI.Instance.AdvanceDialogue();

            //When dialogue ends → drop card ONCE
            if (!stillTalking && !player.cardInventory.HasCard(npcCard))
            {
                player.cardInventory.AddCard(npcCard);
                UI_CardPopup.Instance.ShowCards(new[] { npcCard });
                AudioManager.Instance.PlaySFX(AudioManager.Instance.cardAcquire);
            }

            return;
        }
    }

}
