using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BookOpenScript : MonoBehaviour
{
    //THIS CODE WAS MADE 100% HUMAN MADE, NO BULLSHIT AI WAS USED SINCE I JUST USED MY FUCKING BRAIN (and the help of kevin and alex)
    public Animator animator;
    public Animator SettingsANIM; // first page turner to settings
    public Animator BookMove;
    public Animator Camera;
    public GameObject BookOpen;
    public GameObject Buttons;
    public GameObject Options;
    public GameObject OptionsButton;
    public GameObject Text;
    [SerializeField] private AudioClip[] AudioClips; 
    [SerializeField] private AudioMixer SFXMixer; //take a wild fucking guess at what these are
    [SerializeField] private AudioMixer MusicMixer;
    public AudioSource AudioSource; // the book page turn audio plauer
    public AudioSource UI;
    public Animator ANIMATOR2; // yo remember this is temp, make an array with a for each loop later rayan, ok?
    public GameObject LevelButtons;
    public GameObject Underline; // the underline under back
    public GameObject LevelSelectInteractbles1; // the level icons in settings page
    public GameObject LevelSelectInteractbles2; // the level icons in the level select page
    public GameObject SettingsText; // the sprites of quit and credits in the settings page
    public GameObject SliderShit; // the sprites of sliders
    public SpriteRenderer Credits;
    bool creditsRunner = false;
    private float timer = 0;
    int temp = 0;
    int temp2 = 255;
    public GameObject Sliders;
    private float musicvol;
    private float sfxvol;
    public Slider SFXSlider;
    public Slider musicSlider;
    public Image SliderPic1;
    public Image SliderPic2;
    private bool SliderShower;
    private bool SliderCloser;


    public void Start() // turns the cursor back to normal
    {
        //Cursor.visible = true;
        LoadPrefsSFX();
        LoadPrefsMusic();
        Credits.color = Credits.GetComponent<SpriteRenderer>().color;
        SliderPic1.color = SliderPic1.GetComponent<Image>().color;
        SliderPic2.color = SliderPic2.GetComponent<Image>().color;
    }

    private void Update()
    {
        if (creditsRunner)
        {
            timer += Time.deltaTime;
            if (timer >= 0.001f && temp < 255)
            {
                temp++;
                Credits.color = new Color(Credits.color.r, Credits.color.g, Credits.color.b, temp / 255f);
                timer = 0f;
            }
            else if (temp == 255)
            {
                creditsRunner = false;
                temp = 0;
            }
        }
        if (SliderShower)
        {
            timer += Time.deltaTime;
            if (timer >= 0.001f && temp < 255)
            {
                temp += 5;
                SliderPic1.color = new Color(SliderPic1.color.r, SliderPic1.color.g, SliderPic1.color.b, temp / 255f);
                SliderPic2.color = new Color(SliderPic2.color.r, SliderPic2.color.g, SliderPic2.color.b, temp / 255f);
                timer = 0f;
            }
            else if (temp == 255)
            {
                SliderShower = false;
                temp = 0;
            }
        }
        if (SliderCloser)
        {
            timer += Time.deltaTime;
            if (timer >= 0.001f && temp2 > 0)
            {
                temp2 -= 5;
                SliderPic1.color = new Color(SliderPic1.color.r, SliderPic1.color.g, SliderPic1.color.b, temp2 / 255f);
                SliderPic2.color = new Color(SliderPic2.color.r, SliderPic2.color.g, SliderPic2.color.b, temp2 / 255f);
                timer = 0f;
            }
            else if (temp2 == 0)
            {
                SliderCloser = false;
                temp2 = 0;
            }
        }
    }
    public void TempCredits() // shows the credit UI
    {
        creditsRunner = true;
    }

    public void MusicVolume(float volume) //take a wild fucking guess at what these are
    {
        MusicMixer.SetFloat("MusicVol", volume);
        musicvol = volume;
        SavePrefsMusic(volume);
    } 
    public void SFXVolume(float volume)
    {
        SFXMixer.SetFloat("SFXVol", volume);
        sfxvol = volume;
        SavePrefsSFX(volume);

    } //take a wild fucking guess at what these are

    private void SavePrefsMusic(float volume)
    {
        PlayerPrefs.SetFloat("MusicVol", volume);
    }
    private void SavePrefsSFX(float volume)
    {
        PlayerPrefs.SetFloat("SFXVol", volume);
    }
    private void LoadPrefsMusic()
    {
        var newVolume = PlayerPrefs.GetFloat("MusicVol");
        MusicMixer.SetFloat("MusicVol", newVolume);
        MusicVolume(newVolume);
    }
    private void LoadPrefsSFX()
    {
        
        var newVolume1 = PlayerPrefs.GetFloat("MusicVol");
        var newVolume2 = PlayerPrefs.GetFloat("SFXVol");
        SFXMixer.SetFloat("SFXVol", newVolume1);
        MusicMixer.SetFloat("MusicVol", newVolume2);
        SFXSlider.value = newVolume1;
        musicSlider.value = newVolume2;
    }

    public void Button()
    {
        StartCoroutine(joe());
        BookOpen.SetActive(false);
    }

    public void StartMenu()
    {
        Options.SetActive(true);
        Buttons.SetActive(false);
        AudioSource.pitch = Random.Range(0.9f, 1.1f);
        UI.Play();
    }

    public void LevelSelect1()
    {
        StartCoroutine(joe4());
    }

    public void QuitMenu()
    {
        Application.Quit();
    }

    public void OptionsIN()
    {
        StartCoroutine(joe2());
    }
    public void OptionsOUT()
    {
        StartCoroutine(joe3());
    }

    public void QuitLevel()
    {
        
        StartCoroutine(joe5());
    }

    public void LevelLoader(int levelIndex) //Loads the damn leveluh
    {
        SceneManager.LoadSceneAsync(levelIndex);
    }

    IEnumerator joe() //Book open Couroutine
    {
        AudioSource.clip = AudioClips[3];
        AudioSource.Play();
        BookMove.SetTrigger("Move");
        yield return new WaitForSeconds(0.40f);
        Camera.SetTrigger("Zoom");
        animator.SetTrigger("AnimS");
        AudioSource.clip = AudioClips[0];
        AudioSource.Play();
        yield return new WaitForSeconds(1.34f);
        Text.SetActive(false);
        Buttons.SetActive(true);
    }

    IEnumerator joe2() // Options open
    {
        AudioSource.pitch = Random.Range(0.9f, 1.1f);
        UI.Play();
        Buttons.SetActive(false);
        Text.SetActive(true);
        SettingsANIM.SetTrigger("BookOpen");
        AudioSource.clip = AudioClips[1];
        AudioSource.pitch = Random.Range(0.9f, 1.1f);
        AudioSource.Play();
        yield return new WaitForSeconds(0.80f);
        SettingsText.SetActive(false);
        OptionsButton.SetActive(true);
        SliderShit.SetActive(true);
        Sliders.SetActive(false);
        SliderShower = true;


    }
    IEnumerator joe3() // Options close
    {
        AudioSource.pitch = Random.Range(0.9f, 1.1f);
        UI.Play();
        SliderCloser = true;
        OptionsButton.SetActive(false);
        SettingsText.SetActive(true);
        Sliders.SetActive(true);
        SettingsANIM.SetTrigger("BookClose");
        AudioSource.clip = AudioClips[2];
        AudioSource.pitch = Random.Range(0.9f, 1.1f);
        AudioSource.Play();
        yield return new WaitForSeconds(0.40f);
        SliderShit.SetActive(false);
        yield return new WaitForSeconds(0.80f);
        Credits.color = new Color(1f, 1f, 1f, 0);
        //SliderPic2.color = new Color(1f, 1f, 1f, 0);
        //SliderPic1.color = new Color(1f, 1f, 1f, 0);
        Text.SetActive(false);
        Buttons.SetActive(true);
    }

    IEnumerator joe4() //Opens to level select
    {
        Buttons.SetActive(false);
        Text.SetActive(true);
        AudioSource.pitch = Random.Range(0.9f, 1.1f);
        UI.Play();
        AudioSource.clip = AudioClips[2];
        AudioSource.Play();
        SettingsANIM.SetTrigger("BookOpen");
        yield return new WaitForSeconds(0.03f);
        ANIMATOR2.SetTrigger("BookOpen");
        yield return new WaitForSeconds(0.77f);
        LevelSelectInteractbles1.SetActive(false);
        LevelSelectInteractbles2.SetActive(false);
        LevelButtons.SetActive(true);
    }

    IEnumerator joe5() //Closes to level select
    {
        LevelSelectInteractbles1.SetActive(true);
        LevelSelectInteractbles2.SetActive(true);
        LevelButtons.SetActive(false);
        Underline.SetActive(true);
        Text.SetActive(true);
        AudioSource.pitch = Random.Range(0.9f, 1.1f);
        UI.Play();
        AudioSource.clip = AudioClips[2];
        AudioSource.Play();
        ANIMATOR2.SetTrigger("BookClose");
        yield return new WaitForSeconds(0.03f);
        SettingsANIM.SetTrigger("BookClose");
        yield return new WaitForSeconds(0.77f);
        Text.SetActive(false);
        Buttons.SetActive(true);
        Underline.SetActive(false);
    }

    //IEnumerator joe6()
    //{
    //    color.lerp (Credits.color = new Color(1f, 1f, 1f, 50));
    //    yield return new WaitForSeconds(0.2f);
    //    UI.Play();
    //    Credits.color = new Color(1f, 1f, 1f, 130);
    //    yield return new WaitForSeconds(0.3f);
    //    UI.Play();
    //    Credits.color = new Color(1f, 1f, 1f, 200);
    //    yield return new WaitForSeconds(0.3f);
    //    UI.Play();
    //    Credits.color = new Color(1f, 1f, 1f, 255);
    //    yield return new WaitForSeconds(0.3f);
    //    UI.Play();
    //}
}
