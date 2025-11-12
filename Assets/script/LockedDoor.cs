using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LockedDoor : MonoBehaviour, IInteractable
{
    [Header("Door Setup")]
    public Transform destinationPoint;   // where to teleport player
    public GameObject hintUI;
    public GameObject lockedMessageUI;
    public CardSY requiredCard;

    private bool isUnlocked = false;
    private GameObject player; // reference to the player GameObject
    public bool end;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (lockedMessageUI != null)
            lockedMessageUI.SetActive(false);
        if (hintUI != null)
            hintUI.SetActive(false);
    }

    public void Interact(PlayerInteraction playerInteraction)
    {
        if (!isUnlocked && playerInteraction.cardInventory.HasCard(requiredCard))
        {
            UnlockDoor();
            TryEnter();
        }
        else if (isUnlocked)
        {
            TryEnter();
        }
        else
        {
            ShowLockedMessage();
        }
    }

    private void TryEnter()
    {
        if (destinationPoint != null && player != null)
        {
            StartCoroutine(TeleportWithFade());
        }
        else if (end)
        {
            SceneManager.LoadScene(2);
        }
        else
        {
            Debug.LogWarning("⚠️ Destination point or player not assigned!");
        }
    }

    private void UnlockDoor()
    {
        isUnlocked = true;
        Debug.Log("Door unlocked!");
    }

    private void ShowLockedMessage()
    {
        if (lockedMessageUI != null)
        {
            lockedMessageUI.SetActive(true);
            CancelInvoke(nameof(HideLockedMessage));
            Invoke(nameof(HideLockedMessage), 2f);
        }
    }

    private void HideLockedMessage()
    {
        if (lockedMessageUI != null)
            lockedMessageUI.SetActive(false);
    }

    private IEnumerator TeleportWithFade()
    {
        // Fade to black
        if (ScreenFader.Instance != null)
            yield return ScreenFader.Instance.FadeOut();

        // Teleport player
        player.transform.position = destinationPoint.position;

        // Small delay (optional)
        yield return new WaitForSeconds(0.2f);

        // Fade back in
        if (ScreenFader.Instance != null)
            yield return ScreenFader.Instance.FadeIn();
    }
}
