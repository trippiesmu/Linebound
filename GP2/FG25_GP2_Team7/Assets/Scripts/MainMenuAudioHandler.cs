using UnityEngine;

public class MainMenuAudioHandler : MonoBehaviour
{
    private GameObject MusicBox;
    
    
    
    void Awake()
    {
        MusicBox = GameObject.FindWithTag("MusicBox");
        Destroy(MusicBox);
    }





    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
