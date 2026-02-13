using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    [SerializeField] LevelEndStars StarUI;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger");
        if (other.CompareTag("Player"))
        {
            StarUI.LevelFinished = true;
            Time.timeScale = 0;
            StarUI.gameObject.SetActive(true);
        }
    }
}
