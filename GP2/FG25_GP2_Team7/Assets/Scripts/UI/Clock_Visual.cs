using UnityEngine;
using UnityEngine.UI;

public class Clock_Visual : MonoBehaviour
{
    [SerializeField] LevelEndStars LevelEndStars;
    Slider slider;
    [SerializeField] GameObject Pointer;

    float TimeLeft;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (Time.timeSinceLevelLoad <= LevelEndStars.MaxTimeGoal)
        {
            TimeLeft = Time.timeSinceLevelLoad / LevelEndStars.MaxTimeGoal;
            slider.value = TimeLeft;
            Pointer.transform.localRotation = Quaternion.Euler(0, 0, TimeLeft * 360);
        }
    }
}
