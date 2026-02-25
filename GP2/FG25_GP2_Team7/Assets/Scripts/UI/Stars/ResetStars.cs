using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetStars : MonoBehaviour
{

    public void resetStars()
    {
        for(int i = 1; i <= SceneManager.sceneCountInBuildSettings; i++)
        {
            string s = i.ToString();
            Save(s, 000);
        }
    }
    void Save(string s, int stars)
    {
        PlayerPrefs.SetInt(s, stars);
        PlayerPrefs.Save();
    }
}
