using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        activeNPCs = new List<GameObject>();
        BuildRoomDictionary();
        SubscribeToGameStateEvents();
        UpdateStateFromGameStateManager();
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
        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        ClearNPCs();

        yield return null;

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
                    if (schedule.isDead)
                    {
                        npcRotation = Quaternion.Euler(0, 180f, 90f);
                    }
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
        Debug.Log("Clearing NPCs: " + activeNPCs.Count);
        foreach (var npc in activeNPCs)
        {
            if (npc != null)
                Destroy(npc);
        }
        activeNPCs.Clear();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        BuildRoomDictionary();
        RespawnNPCs();
    }
}