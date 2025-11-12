using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public TMP_Text npcNameText;

    private string[] currentLines;
    private int currentIndex;
    private bool isActive;


    void Awake()
    {
        Instance = this;
        dialoguePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartDialogue(DialogueData data)
    {
        Debug.Log("PauseManager found: " + (GamePauseManager.Instance != null));
        if (data == null) return;
        currentLines = data.dialogueLines;
        currentIndex = 0;
        isActive = true;

        GamePauseManager.Instance?.RequestPause("Dialogue");

        npcNameText.text = data.npcName;
        dialoguePanel.SetActive(true);
        dialogueText.text = currentLines[currentIndex];
            
    }

    public bool AdvanceDialogue()
    {
        if (!isActive) return false;

        currentIndex++;

        if(currentIndex < currentLines.Length)
        {
            dialogueText.text = currentLines[currentIndex];
            return true;
        }else
        {
            EndDialogue();
            return false;
        }
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false );
        isActive = false;
        currentLines = null;

        GamePauseManager.Instance?.RequestResume("Dialogue");
    }

    public bool IsActive() => isActive;
}
