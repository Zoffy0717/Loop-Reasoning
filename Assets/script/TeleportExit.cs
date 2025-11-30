using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportExit : MonoBehaviour, IInteractable
{
    public Transform destinationPoint; // entrance door position
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Interact(PlayerInteraction playerInteraction)
    {
        if (player != null && destinationPoint != null)
        {
            player.transform.position = destinationPoint.position;
        }
        else
        {
            Debug.LogWarning("⚠️ TeleportExit: Missing player or destination!");
        }
    }
}