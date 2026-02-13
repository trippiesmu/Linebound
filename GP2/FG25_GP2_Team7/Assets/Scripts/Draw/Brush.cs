using UnityEngine;

public class Brush : MonoBehaviour
{
    public enum LineState
    {
        Straight,
        Curved
    }

    [SerializeField] float drawCost = 1f;
    [Header("Red = 0, Green = 1, Blue = 2")]
    [SerializeField] int index;
    [SerializeField] private LineState lineState;
    [SerializeField] private Draw draw;

    [SerializeField] private float length = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        draw = GameObject.FindGameObjectWithTag("Paint").GetComponent<Draw>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public LineState GetLine()
    {
        return lineState;
    }

    public float GetCost()
    {
        return drawCost;
    }

    public int GetIndex()
    {
        return index;
    }
    public void AddLength(float lengthAdded)
    {
        length += lengthAdded;
        draw.AddTotalCrayon(lengthAdded);
    }

    public void SetLength(float lengthTotal)
    {
        draw.AddTotalCrayon(-length);
        length = lengthTotal;
        draw.AddTotalCrayon(length);
    }

    public float GetLength()
    {
        return length;
    }
}
