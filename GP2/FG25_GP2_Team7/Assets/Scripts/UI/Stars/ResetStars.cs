using UnityEngine;

public class ResetStars : MonoBehaviour
{
    [SerializeField] int LevelCount = 0;

    public void resetStars()
    {
        for(int i = 1; i <= LevelCount; i++)
        {
            string s = "Level_" + i;
            Save(s, 000);
        }
    }
    void Save(string s, int stars)
    {
        PlayerPrefs.SetInt(s, stars);
        PlayerPrefs.Save();
    }
}
