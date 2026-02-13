using UnityEngine;

public class SelectedCrayon : MonoBehaviour
{
    [SerializeField] private Draw draw;
    [SerializeField] private int index = 0;
    [SerializeField] UnityEngine.UI.Image image;
    [SerializeField] UnityEngine.UI.Image child;
    [SerializeField] AudioSource selectedSFX;

    bool SFXPLAY = true;
    int count = 1;
    [Range(0f, 1f)]
    [SerializeField] float SFXVolume = 0.7f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        draw = GameObject.FindGameObjectWithTag("Paint").GetComponent<Draw>();
        image = gameObject.GetComponent<UnityEngine.UI.Image>();
        if (gameObject.transform.childCount > 0)
        {
            child = gameObject.transform.GetChild(0).gameObject.GetComponent<UnityEngine.UI.Image>();
        }
        selectedSFX.volume = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (draw.GetBrushIndex() == index && draw.GetCrayonAmount(index) > 0f && draw.GetBrushIndex() < 3)
        {
            SoundEffect();
            image.enabled = true;
            if (child != null)
            {
                child.enabled = true;
            }
        }
        else if (draw.GetBrushIndex() == index && draw.GetBrushIndex() == 3)
        {
            SoundEffect();
            image.enabled = true;
            if (child != null)
            {
                child.enabled = true;
            }
        }
        else
        {
            image.enabled = false;
            if (child != null)
            {
                child.enabled = false;
            }
            SFXPLAY = true;
        }
    }

    void SoundEffect()
    {
        if (SFXPLAY && count == 1)
        {
            selectedSFX.volume = SFXVolume;
            selectedSFX.pitch = Random.Range(0.8f, 1.1f);
            selectedSFX.Play();
            SFXPLAY = false;
        }
        if (SFXPLAY && count == 0)
        {
            selectedSFX.pitch = Random.Range(0.8f, 1.1f);
            selectedSFX.Play();
            SFXPLAY = false;
            count++;
        }

        //we dont have enough time but this would be the UI Highlight SFX
    }
}
