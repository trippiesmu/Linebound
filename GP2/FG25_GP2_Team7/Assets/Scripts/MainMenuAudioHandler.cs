using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuAudioHandler : MonoBehaviour
{
    private GameObject MusicBox;
    public int Index;
    public AudioLowPassFilter Filter;


    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        MusicBox = GameObject.FindWithTag("MusicBox");
        Destroy(MusicBox);
    }





    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName == "PlayersReady")
        {
            float current = Filter.cutoffFrequency;
            float cutoff = 600f;
            float lerpSpeed = 2f;
            float FuckAssLerp = Mathf.Lerp(current, cutoff, Time.fixedDeltaTime * lerpSpeed);
            Filter.cutoffFrequency = FuckAssLerp;
        }
    }
}
