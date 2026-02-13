using UnityEngine;
using UnityEngine.InputSystem;

public class BrushSelector : MonoBehaviour
{
    [SerializeField] private Draw draw;
    InputAction RedBrush;
    InputAction GreenBrush;
    InputAction BlueBrush;
    InputAction Eraser;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        draw = GameObject.FindGameObjectWithTag("Paint").GetComponent<Draw>();
        RedBrush = InputSystem.actions.FindAction("RedBrush");
        GreenBrush = InputSystem.actions.FindAction("GreenBrush");
        BlueBrush = InputSystem.actions.FindAction("BlueBrush");
        Eraser = InputSystem.actions.FindAction("Eraser");
    }

    // Update is called once per frame
    void Update()
    {
        if (RedBrush.WasPressedThisFrame()) chooseBrush(0);
        if (GreenBrush.WasPressedThisFrame()) chooseBrush(1);
        if (BlueBrush.WasPressedThisFrame()) chooseBrush(2);
        if (Eraser.WasPressedThisFrame()) chooseBrush(3);
    }

    public void chooseBrush(int index)
    {
        draw.SelectBrush(index);
    }
}
