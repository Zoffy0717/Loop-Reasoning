using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CardPopup : MonoBehaviour
{
    public static UI_CardPopup Instance;
    public GameObject popupRoot;

    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ClosePopup();
        }
    }

    public void ShowCards(CardSY[] cards)
    {
        popupRoot.SetActive(true);
        Time.timeScale = 0f;
    }

    void ClosePopup()
    {
        popupRoot.SetActive(false);
        Time.timeScale = 1f;
    }
}
