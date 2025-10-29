using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour, IInteractable
{

    public DialogueData dialogueData;
    public CardSY npcCard;

    private bool cardDroped = false;
    
    public void Interact(PlayerInteraction player)
    {
        if (!DialogueUI.Instance.IsActive())
        {
            DialogueUI.Instance.StartDialogue(dialogueData);

        }
        else
        {
            bool stillTalking = DialogueUI.Instance.AdvanceDialogue();

            if(!stillTalking && !cardDroped)
            {
                player.cardInventory.AddCard(npcCard);
                UI_CardPopup.Instance.ShowCards(new[] {npcCard});
                cardDroped=true;
            }
        }
    }
}
