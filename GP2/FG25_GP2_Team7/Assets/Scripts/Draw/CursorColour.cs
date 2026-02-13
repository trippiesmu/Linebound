using UnityEngine;

public class CursorColour : MonoBehaviour
{
    [SerializeField] Material red;
    [SerializeField] Material green;
    [SerializeField] Material blue;
    [SerializeField] Draw draw;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        draw = GameObject.FindGameObjectWithTag("Paint").GetComponent<Draw>();
    }

    // Update is called once per frame
    void Update()
    {
        if(draw.GetBrushIndex() == 0)
        {
            gameObject.GetComponent<Renderer>().material = red;
        }
        else if (draw.GetBrushIndex() == 1)
        {
            gameObject.GetComponent<Renderer>().material = green;
        }
        else if (draw.GetBrushIndex()== 2)
        {
            gameObject.GetComponent<Renderer>().material = blue;
        }
    }
}
