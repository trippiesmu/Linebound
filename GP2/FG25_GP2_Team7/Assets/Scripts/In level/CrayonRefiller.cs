using System.Collections;
using UnityEngine;

enum Colour
{
    Red, 
    Green, 
    Blue
}

public class CrayonRefiller : MonoBehaviour
{
    [SerializeField] private Draw draw;
    [SerializeField] private Colour colour;
    [SerializeField] private float crayonAmount = 20f;
    [SerializeField] private GameObject Audio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        draw = GameObject.FindGameObjectWithTag("Paint").GetComponent<Draw>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Instantiate(Audio);
            if (colour == Colour.Red)
            {
                draw.AddCrayon(0, crayonAmount);
            }
            else if (colour == Colour.Green)
            {
                draw.AddCrayon(1, crayonAmount);
            }
            else if (colour == Colour.Blue)
            {
                draw.AddCrayon(2, crayonAmount);
            }
            GameObject.Destroy(this.gameObject);
        }
    }


}
