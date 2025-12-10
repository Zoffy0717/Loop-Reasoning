using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class sceneManager : MonoBehaviour
{

    public GameObject introPanel; // assign the black panel
    public TextMeshProUGUI introText;

    public string[] textsPerSecond = new string[]
    {
        "Rain taps softly against the window, and the wooden floor of your apartment creaks beneath your slow, familiar steps. Tonight should have been like any other night—",
        "a cup of warm tea, a few pages of an old book, and the gentle drift into shallow sleep beneath a dim lamp.",
        "But then you found the postcard.A serene valley stretches across the glossy surface: snow-covered peaks, a lake wrapped in dark pines, and a mountain lodge standing quietly under a plume of chimney smoke.",
        "You recognized it instantly—The Lodge.Tucked beside it was a torn piece of a local newspaper, the edges wrinkled and damp from the rain:“Grant Lodge to be auctioned later this month. Rumors persist regarding the unresolved death of its owner.",
        "Your hand pauses, fingertips brushing the rough paper.How long has it been?Ten years? Twenty?Time has blunted many memories, softened others, and buried some entirely—but not that winter night.Not the shouting, the fear,and the truths that were never spoken aloud.",
        "You sit by the window.Outside, night presses against the glass, reflecting your aging silhouette back at you—faint, ghostlike.Something deep within you stirs, a forgotten ache:the lingering weight of a case you never truly closed.",
        "You close your eyes.The wind howls.The lodge returns.Footsteps.A broken glass.A stopped watch.A body in the snow……"
    };

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

        for (int i = 0; i < textsPerSecond.Length; i++)
        {
            introText.text = textsPerSecond[i];
            yield return new WaitForSeconds(3f);
        }

        // Load gameplay scene
        SceneManager.LoadScene(1);
    }

}
