using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportExit : MonoBehaviour, IInteractable
{
    public Transform destinationPoint; // Where to send the player (entrance)
    private GameObject player;
    public string roomID;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Interact(PlayerInteraction playerInteraction)
    {
        if (player != null && destinationPoint != null)
        {
            player.transform.position = destinationPoint.position;

            var gsm = GameStateManager.Instance;

            if (gsm.HasPaidForRoom(roomID))
            {
                return;
            }

            gsm.AdvanceTimeSlot();
            gsm.MarkRoomPaid(roomID);
        }
    }

}