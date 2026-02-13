using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;

public class BouncePad : MonoBehaviour
{
    float hypotenuse;
    float xValue;
    float yValue;
    Vector2 thisPos1;
    Vector2 thisPos2;
    [SerializeField] float bounceForce = 10f;
    [SerializeField] float bounceMod = 1.5f;
    float currentBounceMod = 1f;
    InputAction JumpAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        JumpAction = InputSystem.actions.FindAction("Jump");
    }

    // Update is called once per frame
    void Update()
    {
        if(JumpAction.IsPressed())
        {
            currentBounceMod = bounceMod;
        }
        else if(JumpAction.WasReleasedThisFrame())
        {
            currentBounceMod = 1;
        }
    }

    public void SetAngle(Vector2 pos1, Vector2 pos2)
    {
        thisPos1 = pos1;
        thisPos2 = pos2;
        hypotenuse = Mathf.Sqrt(Mathf.Pow(pos2.x-pos1.x, 2) + Mathf.Pow(pos2.y-pos1.y, 2));
        yValue = (pos2.x - pos1.x) / hypotenuse;
        xValue = -(pos2.y - pos1.y) / hypotenuse;
        Debug.Log(xValue + " " + yValue);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject player;
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject;
            
            if(isLeft(thisPos1, thisPos2, collision.transform.position))
            {
                player.GetComponent<Rigidbody2D>().AddForce(new Vector2(xValue, yValue) * bounceForce * currentBounceMod, ForceMode2D.Impulse);
            }
            else
            {
                player.GetComponent<Rigidbody2D>().AddForce(new Vector2(xValue, yValue) * bounceForce * currentBounceMod * -1, ForceMode2D.Impulse);
            }
        }
    }

    public bool isLeft(Vector2 a, Vector2 b, Vector2 c)
    {
        Debug.Log("Hit side: " + ((b.x - a.x) * (c.y - a.y) - (b.y - a.y) * (c.x - a.x)));
        return (b.x - a.x) * (c.y - a.y) - (b.y - a.y) * (c.x - a.x) > 0;
    }
}
