using UnityEngine;

public static class NonPersistentData
{
    public static bool ReadyChecked { get; set; } = false;
    public static int SceneToLoad { get; set; } = 0;
}
