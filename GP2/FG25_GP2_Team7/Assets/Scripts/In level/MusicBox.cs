using UnityEngine;

public class MusicBox : MonoBehaviour
{
    [SerializeField] private AudioSource musicBox;
    [SerializeField] private AudioClip musicClip;
    public static MusicBox Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
        musicBox.clip = musicClip;
        musicBox.loop = true;
        if (musicBox.isPlaying == false)
        {
            musicBox.Play();
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
