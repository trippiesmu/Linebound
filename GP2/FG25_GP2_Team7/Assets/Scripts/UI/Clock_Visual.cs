using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Clock_Visual : MonoBehaviour
{
    [SerializeField] LevelEndStars LevelEndStars;
    Slider slider;
    [SerializeField] GameObject Pointer;
    private AudioSource Audio;
    private bool DoOnceBool = false;
    private bool AnotherStupidBool = false; // jesus theres a better way
    float TimeLeft;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Audio = GetComponent<AudioSource>();
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (!DoOnceBool && AnotherStupidBool)
        {
            DoOnceBool = true;
            print("whaha");
            Audio.Play();
            
        }
        if (Time.timeSinceLevelLoad <= LevelEndStars.MaxTimeGoal)
        {
            TimeLeft = Time.timeSinceLevelLoad / LevelEndStars.MaxTimeGoal;
            slider.value = TimeLeft;
            Pointer.transform.localRotation = Quaternion.Euler(0, 0, TimeLeft * 360);
        }
        else
        {
            AnotherStupidBool = true;
        }
    }

}
