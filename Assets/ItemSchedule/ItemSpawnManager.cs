using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class ItemSpawnManager : MonoBehaviour
{
    public static ItemSpawnManager Instance;

    [Header("Assign all item schedules")]
    public ItemSchedule[] itemSchedules;

    // roomID → ItemSpawnPoint
    private Dictionary<string, ItemSpawnPoint> roomSpawnPoints;

    // Track spawned items so we can destroy/reload correctly
    private List<GameObject> activeItems = new List<GameObject>();

    // Track picked items to prevent respawn
    private HashSet<string> pickedItemIDs = new HashSet<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        BuildRoomDictionary();

        LoadPickedItemFlags(); 
        SpawnItemsForCurrentState();

        SubscribeToGameStateEvents();
    }

    private void SubscribeToGameStateEvents()
    {
        var gsm = GameStateManager.Instance;
        if (gsm == null) return;

        gsm.OnDayAdvanced += HandleStateChanged;
        gsm.OnTimeAdvanced += HandleStateChanged;
    }

    private void HandleStateChanged()
    {
        RespawnItems();
    }

    // -------------------------------------------------------------
    // Build dictionary: roomID -> ItemSpawnPoint
    // -------------------------------------------------------------
    private void BuildRoomDictionary()
    {
        roomSpawnPoints = new Dictionary<string, ItemSpawnPoint>();

        foreach (var p in FindObjectsOfType<ItemSpawnPoint>())
        {
            if (!roomSpawnPoints.ContainsKey(p.roomID))
                roomSpawnPoints.Add(p.roomID, p);
            else
                Debug.LogWarning($"Duplicate ItemSpawnPoint for roomID: {p.roomID}");
        }
    }

    // -------------------------------------------------------------
    // Respawn logic
    // -------------------------------------------------------------
    private void RespawnItems()
    {
        ClearItems();
        SpawnItemsForCurrentState();
    }

    private void ClearItems()
    {
        foreach (var i in activeItems)
            if (i != null) Destroy(i);

        activeItems.Clear();
    }

    // -------------------------------------------------------------
    // Core spawning logic
    // -------------------------------------------------------------
    public void SpawnItemsForCurrentState()
    {
        ClearItems();

        var gsm = GameStateManager.Instance;
        if (gsm == null)
        {
            Debug.LogError("No GameStateManager found!");
            return;
        }

        DayType day = gsm.currentDay;
        TimePeriod period = gsm.currentPeriod;

        foreach (var schedule in itemSchedules)
        {
            var entry = schedule.schedule.FirstOrDefault(e =>
                e.day == day &&
                e.period == period
            );

            if (entry == null) continue;

            // Prevent respawn if already picked
            if (pickedItemIDs.Contains(schedule.itemID))
                continue;

            // Get the room's spawn controller
            if (!roomSpawnPoints.TryGetValue(entry.roomID, out var room))
            {
                Debug.LogWarning($"No ItemSpawnPoint found for roomID: {entry.roomID}");
                continue;
            }

            // Get anchor transform
            Transform spawnAnchor = room.GetAnchor(entry.anchorIndex);

            if (spawnAnchor == null)
            {
                Debug.LogWarning($"Room '{entry.roomID}' missing anchor index {entry.anchorIndex}");
                continue;
            }

            // Spawn item
            GameObject item = Instantiate(
                schedule.itemPrefab,
                spawnAnchor.position,
                Quaternion.identity
            );

            activeItems.Add(item);
        }
    }

    // Call this when an item is picked up

    public void MarkItemPicked(string itemID)
    {
        if (!pickedItemIDs.Contains(itemID))
            pickedItemIDs.Add(itemID);
    }

    private void LoadPickedItemFlags()
    {
        pickedItemIDs.Clear();
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
        RespawnItems();
    }
}