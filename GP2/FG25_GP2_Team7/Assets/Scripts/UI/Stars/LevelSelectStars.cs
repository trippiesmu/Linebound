using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectStars : MonoBehaviour
{
    [SerializeField] string Level;

    [Header("Star prefabs")]
    [SerializeField] Sprite NotGained;
    [SerializeField] Sprite Gained;

    List<SpriteRenderer> Stars = new List<SpriteRenderer>();

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

    private void OnEnable()
    {
        Stars.Clear();
        foreach (SpriteRenderer child in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            Stars.Add(child);
            child.sprite = NotGained;
        }
        if (Level == "") Debug.Log("Level is not set so stars will not work for this prefab");

        Load();


        if (Stars.Count > 0)
        {
            for(int i = 0; i < TheseStars.StarCount(); i++)
            {
                Stars[i].sprite = Gained;
            }
        }
    }

    void Load()
    {
        int i = PlayerPrefs.GetInt(Level, 0);
        Debug.Log(Level + " had " + i);

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

        TheseStars.Update(star1, star2, star3);
    }
}
