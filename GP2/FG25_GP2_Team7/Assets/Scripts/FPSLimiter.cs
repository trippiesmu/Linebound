using UnityEngine;

public class FPSLimiter : MonoBehaviour
{
    public int FPS = 165;
    
    
    void Start()
    {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = FPS;
    }

    
    void Update()
    {
        
    }
}
