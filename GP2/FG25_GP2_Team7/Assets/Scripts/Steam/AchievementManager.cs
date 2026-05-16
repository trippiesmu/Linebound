using Steamworks;
using Steamworks.Data;
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
        if (!SteamClient.IsValid)
        {
            Debug.LogWarning("Steam not running, can't unlock achievement.");
            return;
        }

        Achievement ach = new Achievement(apiName);
        ach.Trigger();
        Debug.Log("Achievement unlocked: " + apiName);
    }
}