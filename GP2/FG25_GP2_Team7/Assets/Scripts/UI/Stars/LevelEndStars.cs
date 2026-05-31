using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelEndStars : MonoBehaviour
{
    [SerializeField] Draw paint;
    int Level;
    [Header("Star Goals")]
    [HideInInspector] public bool LevelFinished = false;
    //[SerializeField] float paintGoal;
    [SerializeField] public float MaxTimeGoal;
    [SerializeField] GameObject Collectible;
    [Header("Star prefabs")]
    [SerializeField] Texture NotGained;
    [SerializeField] Texture Gained;
    List<RawImage> Stars = new List<RawImage>();
    StarsClass TheseStars = new StarsClass(false, false, false);
    public class StarsClass
    {
        public bool Star1 = false;
        public bool Star2 = false;
        public bool Star3 = false;
        public StarsClass(bool star1, bool star2, bool star3)
        {
            Star1 = star1;
            Star2 = star2;
            Star3 = star3;
        }
        public void Update(bool star1, bool star2, bool star3)
        {
            Star1 = Star1 || star1;
            Star2 = Star2 || star2;
            Star3 = Star3 || star3;
        }
        public int Return()
        {
            int i = 0;
            if (Star1) i += 100;
            if (Star2) i += 10;
            if (Star3) i += 1;
            return i;
        }
        public int StarCount()
        {
            int i = 0;
            if (Star1) i++;
            if (Star2) i++;
            if (Star3) i++;
            return i;
        }
    }
    void Start()
    {
        Time.timeScale = 1;
        paint = GameObject.FindGameObjectWithTag("Paint").GetComponent<Draw>();
        foreach (RawImage child in transform.GetComponentsInChildren<RawImage>())
        {
            Stars.Add(child);
            child.texture = NotGained;
        }
        Level = SceneManager.GetActiveScene().buildIndex;
        Load();
        this.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        int CurrentStars = TheseStars.StarCount();
        int GainedStars = 0;
        bool star1 = false, star2 = false, star3 = false;

        if (LevelFinished)
        {
            star1 = true;
            GainedStars++;

            if (Time.timeSinceLevelLoad <= MaxTimeGoal)
            {
                star2 = true;
                GainedStars++;
            }

            if (!Collectible.activeSelf)
            {
                star3 = true;
                GainedStars++;
            }
        }

        // Always show current run stars
        Stars[0].texture = star1 ? Gained : NotGained;
        Stars[1].texture = star2 ? Gained : NotGained;
        Stars[2].texture = star3 ? Gained : NotGained;

        // Only save if this run is strictly better
        if (GainedStars > CurrentStars)
        {
            TheseStars = new StarsClass(star1, star2, star3);
            Save();
        }

        // 3 star achievement — only if current run has 3
        if (LevelFinished && GainedStars == 3)
        {
            if (AchievementManager.Instance != null)
                AchievementManager.Instance.UnlockAchievement("ACH_3STAR_LEVEL_" + Level);
        }

        // 100% achievement — only if current run has 3
        if (LevelFinished && GainedStars == 3 && AllLevelsThreeStars())
        {
            if (AchievementManager.Instance != null)
                AchievementManager.Instance.UnlockAchievement("ACH_100_PERCENT");
        }
    }
    void Save()
    {
        PlayerPrefs.SetInt(Level.ToString(), TheseStars.Return());
        PlayerPrefs.Save();
    }
    void Load()
    {
        int i = PlayerPrefs.GetInt(Level.ToString(), 0);
        bool star1 = false, star2 = false, star3 = false;
        if (i >= 100)
        {
            star1 = true;
            if (i >= 110)
            {
                star2 = true;
                if (i == 111) star3 = true;
            }
            else if (i == 101) star3 = true;
        }
        else if (i >= 10)
        {
            star2 = true;
            if (i == 11) star3 = true;
        }
        else if (i == 1) star3 = true;
        TheseStars = new StarsClass(star1, star2, star3);
    }
    bool AllLevelsThreeStars()
    {
        for (int i = 1; i <= 15; i++)
        {
            int saved = PlayerPrefs.GetInt(i.ToString(), 0);
            if (saved != 111) return false;
        }
        return true;
    }
}