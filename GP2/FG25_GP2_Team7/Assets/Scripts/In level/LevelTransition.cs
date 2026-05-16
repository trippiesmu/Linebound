using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    [SerializeField] LevelEndStars StarUI;
    [SerializeField] AudioSource WinSFX;
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger");
        if (other.CompareTag("Player"))
        {
            WinSFX.Play();
            StarUI.LevelFinished = true;
            Time.timeScale = 0;
            this.GetComponent<Collider2D>().enabled = false;
            StarUI.gameObject.SetActive(true);
        }
    }
}
