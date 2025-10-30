using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class sceneManager : MonoBehaviour
{

    public GameObject introPanel; // assign the black panel
    public float introDuration = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    public void OnButtonClicked()
    {
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        // Show black screen + text
        introPanel.SetActive(true);

        // Wait 5 seconds
        yield return new WaitForSeconds(introDuration);

        // Load gameplay scene
        SceneManager.LoadScene(1);
    }

}
