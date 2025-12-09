using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEntrance : MonoBehaviour, IInteractable
{
    [Header("Room Info")]
    public string roomID;       
    public int enterCost = 1;   

    [Header("Teleport Options")]
    public Transform destinationPoint;
    private GameObject player;

    public bool triggersStartOfDay1;
    public bool isBedroomEntrance = false;

    public string roomDisplayName;   // What shows in the UI
    public GameObject hintUI;        // same idea as NPC hint

    [Header("Room Card")]
    public CardSY roomCard;     
    private bool hasDroppedCard = false;

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

        // 🔍 Check if AP already paid this today
        if (!gsm.HasPaidForRoom(roomID))
        {
            // Need to pay this time
            if (!gsm.HasEnoughActionPoints(enterCost))
            {
                return;
            }

            gsm.ConsumeAP_NoTimeAdvance(enterCost);
        }
        else
        {
            Debug.Log($"🔁 Re-entering {roomID} in the same time period — FREE");
        }

        // Teleport player
        if (destinationPoint != null && player != null)
            StartCoroutine(TeleportWithFade());
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

        TryDropRoomCard();
    }

    private void TryDropRoomCard()
    {
        if (roomCard == null)
            return;

        if (hasDroppedCard)
            return;

        var inventory = FindObjectOfType<CardInventory>();
        if (inventory != null)
        {
            inventory.AddCard(roomCard);
            UI_CardPopup.Instance.ShowCards(new[] { roomCard });
            hasDroppedCard = true;
            Debug.Log($"🃏 Room card acquired: {roomCard.displayName}");
        }
    }


    /// section for showing hintUI
    public void ShowHint()
    {
        if (hintUI != null)
            hintUI.SetActive(true);
    }

    public void HideHint()
    {
        if (hintUI != null)
            hintUI.SetActive(false);
    }

    public string GetInteractName()
    {
        return roomDisplayName;  // "Kitchen", "Bedroom 003", etc.
    }
}

