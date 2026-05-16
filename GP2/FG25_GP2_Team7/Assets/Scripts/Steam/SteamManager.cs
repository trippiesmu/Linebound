using Steamworks;
using UnityEngine;

public class SteamManager : MonoBehaviour
{
    public static SteamManager Instance;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        try
        {
            SteamClient.Init(4447550);
            Debug.Log("Steam initialized! Playing as: " + SteamClient.Name);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Steam failed to initialize: " + e.Message);
        }
    }

    void Update() => SteamClient.RunCallbacks();

    void OnDestroy() => SteamClient.Shutdown();
}