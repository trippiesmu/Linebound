using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeTESTScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) // Im using on trigger enter for now, but if y'all want you can change it to collision and then pressing a key to enter the door or smth.
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //THIS IS ALL SHIT UNOPTIMIZED CODE, I PUT THIS FOR THE ALPHA
    }
}
