using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{

    public CardInventory cardInventory;
    public float interactRange = 1f;
    public LayerMask interactableMask;
    private IInteractable currentTarget;
    public GameObject interactHintUI;
    private NPCInteractable currentHintNPC;

    private bool isReasoningOpen = false;
    public ReasoningBoardUI reasoningUI;
    // Start is called before the first frame update
    void Start()
    {
        if (interactHintUI != null)
            interactHintUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHintUI();
        if (currentTarget != null)
        {
            var mono = currentTarget as MonoBehaviour;
            if (mono == null || !mono.gameObject.activeInHierarchy)
            {
                currentTarget = null;
                Debug.Log("Cleared invalid currentTarget (object was disabled or destroyed)");
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentTarget != null)
            {
                currentTarget.Interact(this);

                currentTarget = null;

            }
            else
            {
                TryFindAndInteract();
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            isReasoningOpen = !isReasoningOpen;
            reasoningUI.Toggle(isReasoningOpen);
        }

        if (currentHintNPC)
        {
            float dist = Vector2.Distance(transform.position, currentHintNPC.transform.position);
            if (dist > interactRange)
            {
                if (currentHintNPC.interactHintUI)
                    currentHintNPC.interactHintUI.SetActive(false);

                currentHintNPC = null;
            }
        }
    }

    private void TryFindAndInteract()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRange, interactableMask);

        if (hits.Length == 0)
        {
            if (currentHintNPC && currentHintNPC.interactHintUI)
                currentHintNPC.interactHintUI.SetActive(false);

            currentHintNPC = null;
            Debug.Log("No interactables nearby.");
            return;
        }

        // Find the closest one
        Collider2D closest = null;
        float closestDist = Mathf.Infinity;
        foreach (var h in hits)
        {
            float dist = Vector2.Distance(transform.position, h.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = h;
            }
        }
        Debug.Log($"Closest collider: {closest.name} (root:{closest.transform.root.name})");

        if (closest != null)
        {
            var interactable = closest.GetComponent<IInteractable>();

            if (interactable != null)
            {
                currentTarget = interactable;
                Debug.Log($"Interacted with: {closest.name}");

                var npc = closest.GetComponent<NPCInteractable>();
                if (npc != null)
                {
                    // Hide old hint if switching targets
                    if (currentHintNPC && currentHintNPC != npc && currentHintNPC.interactHintUI)
                        currentHintNPC.interactHintUI.SetActive(false);

                    // Show new hint
                    currentHintNPC = npc;
                    if (npc.interactHintUI)
                    {
                        npc.interactHintUI.SetActive(true);
                    }
                }

                interactable.Interact(this);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var node = other.GetComponent<ExplorationNode>();
        if (node != null && node.autoDropOnEnter)
        {
            node.Explore(this);
        }

        var interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            currentTarget = interactable;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var interactable = other.GetComponent<IInteractable>();
        if (interactable != null && interactable == currentTarget)
        {
            currentTarget = null;
        }
    }

    private void UpdateHintUI()
    {
        // Look for all interactables in range
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRange, interactableMask);

        NPCInteractable nearestNPC = null;
        float closestDist = Mathf.Infinity;

        foreach (var h in hits)
        {
            NPCInteractable npc = h.GetComponent<NPCInteractable>();
            if (npc != null)
            {
                Vector2 closestPoint = h.ClosestPoint(transform.position);
                float dist = Vector2.Distance(transform.position, closestPoint);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    nearestNPC = npc;
                }
            }
        }

        // Hide previous hint if switching or leaving range
        if (currentHintNPC != null && currentHintNPC != nearestNPC && currentHintNPC.interactHintUI != null)
            currentHintNPC.interactHintUI.SetActive(false);

        currentHintNPC = nearestNPC;

        // Show the hint if we found a nearby NPC
        if (currentHintNPC != null && currentHintNPC.interactHintUI != null)
            currentHintNPC.interactHintUI.SetActive(true);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }

}
