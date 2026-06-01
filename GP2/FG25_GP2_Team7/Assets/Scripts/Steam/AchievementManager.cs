using Steamworks;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void UnlockAchievement(string apiName)
    {
        if (!SteamClient.IsValid) return;

        var ach = new Steamworks.Data.Achievement(apiName);
        ach.Trigger();
        Debug.Log("Achievement triggered: " + apiName);
    }
}