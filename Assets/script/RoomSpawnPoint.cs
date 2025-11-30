using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawnPoint : MonoBehaviour
{
    
    public string roomID; // must match the ID in the schedule
    [Header("NPC Spawn Anchors")]
    public List<Transform> npcAnchors = new List<Transform>();

    public Transform GetAnchor(int index)
    {
        if (npcAnchors == null || npcAnchors.Count == 0)
            return transform;

        if (index < 0 || index >= npcAnchors.Count)
            return npcAnchors[0];

        return npcAnchors[index];
    }
}
