using UnityEngine;
using UnityEngine.InputSystem;

public class RoundCursor : MonoBehaviour
{
    [SerializeField] GameObject target;
    Vector2 handPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Target");
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Pointer.current.press.IsPressed())
        {
            handPosition = (Pointer.current.position.ReadValue());
        }
        else
        {
            handPosition = (Mouse.current.position.ReadValue());
        }
        target.transform.position = handPosition;
    }
}
