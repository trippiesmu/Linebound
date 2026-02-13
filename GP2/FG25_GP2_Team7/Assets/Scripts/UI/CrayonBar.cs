using UnityEngine;

public class CrayonBar : MonoBehaviour
{
    private float remainingCrayon;
    [SerializeField] private int index;
    [SerializeField] private Draw draw;
    [SerializeField] float MaxPool = 60f;
    [SerializeField] float PercentToRemove = 0.1f;
    [SerializeField] AudioSource ResourceDepletedSFX;
    private RectTransform rectTransform;
    bool SFXPLAY = true;
    int count = 0;
    [Range(0f, 1f)]
    [SerializeField] float SFXVolume = 0.7f;


    UnityEngine.UI.Image image;
    UnityEngine.UI.Image Child;
    float MaxY;
    float MaxHeight;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        draw = GameObject.FindGameObjectWithTag("Paint").GetComponent<Draw>();
        rectTransform = gameObject.GetComponent<RectTransform>();
        MaxHeight = rectTransform.rect.height;
        MaxY = rectTransform.rect.y;
        image = gameObject.GetComponent<UnityEngine.UI.Image>();
        Child = gameObject.transform.GetChild(0).gameObject.GetComponent<UnityEngine.UI.Image>();
        ResourceDepletedSFX.volume = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (draw.GetCrayonAmount(index) / MaxPool > PercentToRemove)
        {

            rectTransform.sizeDelta = new Vector2(rectTransform.rect.width, Mathf.Clamp(MaxHeight * (draw.GetCrayonAmount(index) + (MaxPool / 5)) / MaxPool, 50, MaxHeight));
            image.enabled = true;
            Child.enabled = true;
            SFXPLAY = true;
        }
        else
        {
            image.enabled = false;
            Child.enabled = false;
            SoundEffect();
        }
    }

    void SoundEffect()
    {
        if (SFXPLAY && count == 1)
        {
            ResourceDepletedSFX.volume = SFXVolume;
            ResourceDepletedSFX.pitch = Random.Range(0.8f, 1.1f);
            ResourceDepletedSFX.Play();
            SFXPLAY = false;
        }
        if (SFXPLAY && count == 0)
        {
            ResourceDepletedSFX.pitch = Random.Range(0.8f, 1.1f);
            ResourceDepletedSFX.Play();
            SFXPLAY = false;
            count++;
        }

    }
}
