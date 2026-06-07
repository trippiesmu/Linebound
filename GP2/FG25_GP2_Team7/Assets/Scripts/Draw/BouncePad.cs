using UnityEngine;
using UnityEngine.InputSystem;

public class BouncePad : MonoBehaviour
{
    [SerializeField] Draw draw;

    // Exposed so you can view and manually assign in the Inspector
    [SerializeField] Vector2 pos1;
    [SerializeField] Vector2 pos2;

    float hypotenuse;
    float xValue;
    float yValue;

    [SerializeField] float bounceForce = 10f;
    [SerializeField] float bounceMod = 1.5f;
    float currentBounceMod = 1f;
    InputAction JumpAction;

    // Buffer window (seconds) that allows a jump press shortly before collision to count
    [SerializeField] float bounceInputBuffer = 0.3f;
    float lastJumpPressedTime = Mathf.NegativeInfinity;

    void Start()
    {
        JumpAction = InputSystem.actions.FindAction("Jump");
        draw = GameObject.FindGameObjectWithTag("Paint").GetComponent<Draw>();
        ComputeAngle(); // ensure values are computed at start if set in inspector
    }

    void Update()
    {
        // Track the time of the most recent jump press for input buffering
        if (JumpAction.WasPressedThisFrame())
        {
            lastJumpPressedTime = Time.time;
        }

        if (JumpAction.IsPressed())
        {
            currentBounceMod = bounceMod;
        }
        else if (JumpAction.WasReleasedThisFrame())
        {
            currentBounceMod = 1;
        }
    }

    // Keep API for runtime callers; also updates the serialized values so they remain visible in the Inspector
    public void SetAngle(Vector2 a, Vector2 b)
    {
        pos1 = a;
        pos2 = b;
        ComputeAngle();
    }

    // Recompute whenever you change pos1/pos2 in the Inspector
    void OnValidate()
    {
        ComputeAngle();
    }

    void ComputeAngle()
    {
        hypotenuse = Mathf.Sqrt(Mathf.Pow(pos2.x - pos1.x, 2) + Mathf.Pow(pos2.y - pos1.y, 2));
        if (hypotenuse == 0f)
        {
            xValue = 0f;
            yValue = 0f;
            return;
        }

        yValue = (pos2.x - pos1.x) / hypotenuse;
        xValue = -(pos2.y - pos1.y) / hypotenuse;
#if UNITY_EDITOR
        Debug.Log($"BouncePad angle vector: ({xValue}, {yValue})");
#endif
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject player;
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject;

            // Determine whether to apply the bounce mod:
            // - if jump is currently held, or
            // - if jump was pressed within the buffer window right before the collision
            bool jumpBuffered = (Time.time - lastJumpPressedTime) <= bounceInputBuffer;
            bool jumpHeld = JumpAction.IsPressed();
            float effectiveMod = (jumpHeld || jumpBuffered) ? bounceMod : 1f;

            // Consume the buffered press so it isn't reused for subsequent collisions
            if (jumpBuffered && !jumpHeld)
            {
                lastJumpPressedTime = Mathf.NegativeInfinity;
            }

            if (draw.GetMoonJumpBool())
            {
                if (isLeft(pos1, pos2, collision.transform.position))
                {
                    player.GetComponent<Rigidbody2D>().AddForce(new Vector2(xValue, yValue) * bounceForce * effectiveMod, ForceMode2D.Impulse);
                }
                else
                {
                    player.GetComponent<Rigidbody2D>().AddForce(new Vector2(xValue, yValue) * bounceForce * effectiveMod * -1, ForceMode2D.Impulse);
                }
            }
            else if (!draw.GetMoonJumpBool())
            {
                if (isLeft(pos1, pos2, collision.transform.position))
                {
                    player.GetComponent<Rigidbody2D>().AddForce(new Vector2(xValue, 0) * bounceForce * effectiveMod, ForceMode2D.Impulse);
                    player.GetComponent<Rigidbody2D>().linearVelocityY = yValue * bounceForce * effectiveMod;
                }
                else
                {
                    player.GetComponent<Rigidbody2D>().AddForce(new Vector2(xValue, 0) * bounceForce * effectiveMod * -1, ForceMode2D.Impulse);
                    player.GetComponent<Rigidbody2D>().linearVelocityY = yValue * bounceForce * effectiveMod * -1;
                }
            }

        }
    }

    public bool isLeft(Vector2 a, Vector2 b, Vector2 c)
    {
        Debug.Log("Hit side: " + ((b.x - a.x) * (c.y - a.y) - (b.y - a.y) * (c.x - a.x)));
        return (b.x - a.x) * (c.y - a.y) - (b.y - a.y) * (c.x - a.x) > 0;
    }
}
