using UnityEngine;

public class CrayonBarHighlight : MonoBehaviour
{
    private float remainingCrayon;
    [SerializeField] private int index;
    [SerializeField] private Draw draw;
    [SerializeField] float maxPool = 60f;
    private RectTransform rectTransform;
    [SerializeField] AudioSource SelectedCrayon;
    bool SFXPLAY = true;
    int count = 0;
    [Range(0f, 1f)]
    [SerializeField] float SFXVolume = 0.7f;
    UnityEngine.UI.Image image;
    UnityEngine.UI.Image child;
    float maxX;
    float maxWidth;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        draw = GameObject.FindGameObjectWithTag("Paint").GetComponent<Draw>();
        rectTransform = gameObject.GetComponent<RectTransform>();
        maxWidth = rectTransform.rect.width;
        maxX = rectTransform.rect.x;
        image = gameObject.GetComponent<UnityEngine.UI.Image>();
        if (gameObject.transform.childCount > 0)
        {
            child = gameObject.transform.GetChild(0).gameObject.GetComponent<UnityEngine.UI.Image>();
        }
        SelectedCrayon.volume = 0;
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.sizeDelta = new Vector2(Mathf.Clamp(maxWidth * (draw.GetCrayonAmount(index) + (maxPool / 5)) / maxPool, 50, maxWidth), rectTransform.rect.height);
        /*if(draw.GetCrayonAmount(index) <= 0)
        {
            image.enabled = false;
            if (child != null)
            {
                child.enabled = false;
            }
            SFXPLAY = true;
        }
        else
        {
            SoundEffect();
            image.enabled = true;
            if (child != null)
            {
                child.enabled = true;
            }
        }*/
    }
}
