using UnityEngine;

public class Collectible : MonoBehaviour
{
    BoxCollider2D boxCollider;
    public GameObject Audio;
    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Instantiate(Audio);
            this.gameObject.SetActive(false);
        }
    }
}
