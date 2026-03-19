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

    public CameraFollow cameraFollow;
    public Collider2D roomBounds;
    private GameStateManager gsm;

    [Header("Room Card")]
    public CardSY roomCard;     
    private bool hasDroppedCard = false;

    public AudioClip roomMusic;
    public bool useOverrideMusic = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gsm = GameStateManager.Instance;
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
            {
                StartCoroutine(DayFade());
            }
                

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
            StartCoroutine(DayFade());
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

        cameraFollow.SetRoomBounds(roomBounds);
        if (roomMusic != null)
        {
            if (useOverrideMusic)
            {
                AudioManager.Instance.PlayOverrideMusic(roomMusic);
            }
            else
            {
                AudioManager.Instance.PlayMusic(roomMusic);
            }
        }
        else
        {
            // If no room music, stop override (go back to normal)
            AudioManager.Instance.StopOverrideMusic();
        }
        yield return new WaitForSeconds(0.1f);

        

        if (ScreenFader.Instance != null)
            yield return ScreenFader.Instance.FadeIn();

        TryDropRoomCard();
    }

    private IEnumerator DayFade()
    {
        if (gsm.currentDay == DayType.Day2)
        {
            gsm.StartNextDay();
        }

        if (ScreenFader.Instance != null)
            yield return ScreenFader.Instance.FadeOut();

        if (gsm.currentDay == DayType.Day0)
        {
            gsm.StartDay1();
        }
        else
        {
            gsm.StartNextDay();
        }
            yield return new WaitForSeconds(0.1f);

        if (ScreenFader.Instance != null)
            yield return ScreenFader.Instance.FadeIn();
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

