using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEntrance : MonoBehaviour, IInteractable
{
    [Header("Room Info")]
    public string roomID;       // Example: "Kitchen", "Garage", "Bedroom"
    public int enterCost = 1;   // AP cost (0 = free rooms such as hallway)

    [Header("Teleport Options")]
    public Transform destinationPoint;
    private GameObject player;

    public bool triggersStartOfDay1;
    public bool isBedroomEntrance = false;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Interact(PlayerInteraction playerInteraction)
    {
        TryEnterRoom();
    }

    private void TryEnterRoom()
    {
        GameStateManager gsm = GameStateManager.Instance;

        if (gsm.currentDay == DayType.Day0)
        {
            if (triggersStartOfDay1)
                gsm.StartDay1();

            if (enterCost == 0)
            {
                EnterRoomNoCost();
                return;
            }
            else
            {
                Debug.Log("❌ Cannot enter AP-cost rooms on Day 0.");
                return;
            }
        }

        if (isBedroomEntrance && gsm.actionPointsRemaining == 0 && gsm.currentPeriod == TimePeriod.Night)
        {
            gsm.StartNextDay();
            return;
        }

        // 🔍 Check if AP already paid this time slot
        if (!gsm.HasPaidForRoom(roomID))
        {
            // Need to pay this time
            if (!gsm.HasEnoughActionPoints(enterCost))
            {
                Debug.Log("❌ Not enough AP to enter " + roomID);
                return;
            }

            gsm.UseActionPoints(enterCost);
            gsm.MarkRoomPaid(roomID);
        }
        else
        {
            Debug.Log($"🔁 Re-entering {roomID} in the same time period — FREE");
        }

        gsm.NotifyRoomEntered(roomID);

        // Teleport player
        if (destinationPoint != null && player != null)
            player.transform.position = destinationPoint.position;

        Debug.Log($"➡️ Entered room: {roomID}. AP left: {gsm.ActionPointsRemaining}");
    }

    private void EnterRoomNoCost()
    {
        if (destinationPoint != null && player != null)
        {
            StartCoroutine(TeleportWithFade());
        }
        else
        {
            Debug.LogWarning("⚠️ Missing player or destination point!");
        }
    }

    private IEnumerator TeleportWithFade()
    {
        if (ScreenFader.Instance != null)
            yield return ScreenFader.Instance.FadeOut();

        player.transform.position = destinationPoint.position;

        yield return new WaitForSeconds(0.1f);

        if (ScreenFader.Instance != null)
            yield return ScreenFader.Instance.FadeIn();
    }
}

