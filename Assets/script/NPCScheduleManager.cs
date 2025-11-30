using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class NPCScheduleManager : MonoBehaviour
{
    public static NPCScheduleManager Instance;

    public int currentChapter = 0;
    public string currentTimeSlot = "Morning";

    public NPCSchedule[] npcSchedules;  // assign all NPC schedule SOs
    public Dictionary<string, Transform> roomSpawnPoints;

    private List<GameObject> activeNPCs = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        BuildRoomDictionary();
        SubscribeToGameStateEvents();
        UpdateStateFromGameStateManager();
        SpawnNPCsForCurrentState();
    }

    private void SubscribeToGameStateEvents()
    {
        GameStateManager gsm = GameStateManager.Instance;
        if (gsm == null) return;

        gsm.OnDayAdvanced += HandleDayAdvanced;
        gsm.OnTimeAdvanced += HandleTimeAdvanced;
    }

    private void UpdateStateFromGameStateManager()
    {
        GameStateManager gsm = GameStateManager.Instance;
        if (gsm == null) return;

        // Tie NPC system to GameStateManager day → chapter logic
        currentChapter = (int)gsm.currentDay; // Day0 → chapter=0, Day1=1, Day2=2
        currentTimeSlot = gsm.currentPeriod.ToString(); // "Morning", "Noon", "Night"
    }

    private void HandleDayAdvanced()
    {
        UpdateStateFromGameStateManager();
        RespawnNPCs();
    }

    private void HandleTimeAdvanced()
    {
        UpdateStateFromGameStateManager();
        RespawnNPCs();
    }

    private void RespawnNPCs()
    {
        ClearNPCs();
        SpawnNPCsForCurrentState();
    }


    void BuildRoomDictionary()
    {
        roomSpawnPoints = new Dictionary<string, Transform>();

        foreach (var room in FindObjectsOfType<RoomSpawnPoint>())
        {
            if (!roomSpawnPoints.ContainsKey(room.roomID))
                roomSpawnPoints.Add(room.roomID, room.transform);
        }
    }

    public void SpawnNPCsForCurrentState()
    {
        ClearNPCs();

        foreach (var schedule in npcSchedules)
        {
            var entry = schedule.schedule.FirstOrDefault(s =>
                s.chapter == currentChapter &&
                s.timeSlot == currentTimeSlot
            );

            if (entry != null)
            {
                if (roomSpawnPoints.TryGetValue(entry.roomID, out Transform room))
                {
                    RoomSpawnPoint sp = room.GetComponent<RoomSpawnPoint>();

                    Transform anchor = sp.GetAnchor(entry.anchorIndex);

                    Vector3 pos = anchor.position;

                    Quaternion npcRotation = Quaternion.Euler(0, 180f, 0);

                    GameObject npc = Instantiate(
                        schedule.npcPrefab,
                        pos,
                        npcRotation
                    );

                    activeNPCs.Add(npc);
                }
            }
        }
    }

    void ClearNPCs()
    {
        foreach (var npc in activeNPCs)
        {
            if (npc != null)
                Destroy(npc);
        }
        activeNPCs.Clear();
    }
}