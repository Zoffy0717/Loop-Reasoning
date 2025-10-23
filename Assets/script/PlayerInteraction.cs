using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{

    public CardInventory cardInventory;
    public float interactRange = 1f;
    public LayerMask interactableMask;
    private IInteractable currentTarget;
    private bool isReasoningOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentTarget != null)
            {
                currentTarget.Interact(this);
            }
            else
            {
                TryFindAndInteract();
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            isReasoningOpen = !isReasoningOpen;
        }
    }

    private void TryFindAndInteract()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRange, interactableMask);

        if (hits.Length == 0)
        {
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

        if (closest != null)
        {
            var interactable = closest.GetComponent<IInteractable>();
            if (interactable != null)
            {
                currentTarget = interactable;
                interactable.Interact(this);
                Debug.Log($"Interacted with: {closest.name}");
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

    private void OnTriggerExist2D(Collider2D other)
    {
        var interactable = other.GetComponent<IInteractable>();
        if (interactable != null && interactable == currentTarget)
        {
            currentTarget = null;
        }
    }
}
