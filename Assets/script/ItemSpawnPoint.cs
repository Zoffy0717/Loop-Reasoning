using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnPoint : MonoBehaviour
{
    public string roomID;
    public List<Transform> anchors = new List<Transform>();

    public Transform GetAnchor(int index)
    {
        if (anchors == null || anchors.Count == 0)
        {
            Debug.LogError($"Room {roomID} has NO anchors!");
            return transform; // fallback
        }

        if (index < 0 || index >= anchors.Count)
            return anchors[anchors.Count - 1];

        return anchors[index];
    }
}
