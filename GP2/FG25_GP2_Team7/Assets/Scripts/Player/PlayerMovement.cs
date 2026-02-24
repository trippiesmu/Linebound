using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    InputAction MoveAction;
    InputAction JumpAction;
    InputAction ExtraAnimation;
    InputAction RestartAction;
    InputAction ResetStars;
    SpriteRenderer spriteRenderer;
    Animator animator;

    //Used to see if the player gets to jump
    bool Grounded;
    bool JumpCheck = false;
    bool JumpCooldownBool = false;
    Bounds colliderSize;
    Coroutine Coyote;
    Coroutine Jumping;
    bool OnGreen;
    Coroutine InAir;

    Vector2 Movement = Vector2.zero;

    [Header("Gavity Controls")]
    [SerializeField] float Gravity = 1;
    [SerializeField] float MaxGravity = 5;
    [SerializeField] float BonusGroundedGravity = 4;
    [SerializeField] float JumpingGravity = 1;
    [SerializeField] float MaxJumpingGravity = 5;

    [Header("Jumping controls")]
    [SerializeField] float JumpForce = 5;
    float TrueJumpHeight => JumpForce;
    [SerializeField] float MinimumJumpDuration = 0.1f;
    Coroutine MinJumpTime;
    [SerializeField] float MaximumJumpDuration = 1f;
    [SerializeField] float CoyoteTime = 0.5f;
    [SerializeField] float JumpBufferTimer = 1;
    [SerializeField] float JumpCooldown = 0.25f;
    Coroutine JumpBuffer;
    [SerializeField][Range(0f, 90f)] float MaxJumpAngle = 45f;

    [Header("GroundMovement")]
    [SerializeField] float MaxSpeed = 5;
    [SerializeField][Range(0f, 1f)] float GroundAcceleration = 2;
    [SerializeField][Range(0f, 1f)] float GroundDecceleration = 2;
    float TrueGroundAcceleration => GroundAcceleration * MaxSpeed;
    float TrueGroundDecceleration => GroundDecceleration * MaxSpeed;
    float GroundDownRange = 0.1f;

    
    [Header("SlopeControls")]
    [SerializeField] float SlopeForwardRange = 5f;
    private Vector2 SlopeNormalPerp;
    private float SlopeDownAngle;
    bool OnSlope = false;
    bool LookingAtSlope = false;
    [SerializeField] bool SlopeAngleDebug = false;
    [SerializeField][Range(0f, 90f)] float MinAngleSlide = 20f;
    [SerializeField][Range(0f, 90f)] float MaxAngleslide = 90f;
    [SerializeField] float GravityAtMin = 0f;
    [SerializeField] float GravityAtMax = 5f;
    [SerializeField] float StaticExtraGravity = 0.1f;
    private float SlopeForwardAngle;
    LayerMask mask;
    float SlopeDownRange = 0.3f;

    [Header("AirMovement")]
    [SerializeField][Range(0f, 1f)] float AirAcceleration = 0.1f;
    [SerializeField][Range(0f, 1f)] float AirDecceleration = 0.1f;
    float TrueAirAcceleration => AirAcceleration * TrueGroundAcceleration;
    float TrueAirDecceleration => AirDecceleration * MaxSpeed;

    [Header("Global movement")]
    [SerializeField][Range(-1f, 1f)] float BonusTurnaroundSpeed = 0;

    [Header("RedBrush")]
    [SerializeField] float RedBrushMaxspeedMod = 1.5f;
    [SerializeField] float RedBrushAccelerationMod = 1.5f;
    [SerializeField] float RedBrushGravity = 10f;
    [SerializeField] float RedBrushUpSlopeGravity = 10f;
    bool RedBrush = false;

    [Header("Audio Components")]
    [SerializeField] AudioSource SFXPlayer;
    [SerializeField] AudioSource WalkPlayer;
    [SerializeField] AudioClip[] SFX;
    [Range(0.2f, 0.4f)]
    [SerializeField] float WalkSFXSpeed = 0.28f;
    bool NOTJump = true; // secret variable to make the audio only play once, prolly can be reworked into not being needed, but thats for after the beta
    bool NOTWalk = true;


    [Header("Experimental")]
    [SerializeField] bool StopYMovementOnSlopes = false;
    [SerializeField] bool InconsistentJumpsOnRedSlopes = false;
    [SerializeField] bool ResetStarsOnDel = false;


    CompositeCollider2D composite2D;

    void Start()
    {
        MoveAction = InputSystem.actions.FindAction("Player/Move");
        JumpAction = InputSystem.actions.FindAction("Jump");
        RestartAction = InputSystem.actions.FindAction("Restart");
        ResetStars = InputSystem.actions.FindAction("ResetStars");
        ExtraAnimation = InputSystem.actions.FindAction("Interact");
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        mask = LayerMask.GetMask("Default");
        animator = GetComponent<Animator>();
        composite2D = GetComponent<CompositeCollider2D>();
    }

    void Update()
    {
        Movement = new Vector2(Mathf.RoundToInt(MoveAction.ReadValue<Vector2>().x), 0);
        if (Time.timeScale > 0)
        {
            if (Movement.x < 0) spriteRenderer.flipX = true;
            else if (Movement.x > 0) spriteRenderer.flipX = false;
        }

        if(Movement.magnitude > 0)
        {
            animator.SetBool("Walking", true);
            AudioRunner();
        }
        else if(rb.linearVelocity.x <= 0.01f)
        {
            animator.SetBool("Walking", false);
            AudioRunner();
        }

        V_GroundCheck();
        SlopeCheck();

        if (ResetStarsOnDel)
        {
            if (ResetStars.WasPressedThisFrame())
            {
                for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
                {
                    string s = "Level_" + i;
                    PlayerPrefs.SetInt(s, 000);
                }
                PlayerPrefs.Save();
            }
        }

        if (ExtraAnimation.WasPressedThisFrame())
        {
            animator.SetTrigger("Emote");
        }

        if (RestartAction.WasPressedThisFrame())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Time.timeScale = 1;
        }

        V_Jumping();
        Experimental();

        
    }

    private void FixedUpdate()
    {
        if (Grounded) GroundMove();
        else AirMove();

        V_Gravity();
    }

    void V_Gravity()
    {
        float g = BonusGroundedGravity;
        if (RedBrush) g = RedBrushGravity;

        if (((OnSlope && SlopeDownAngle > 0) || (LookingAtSlope && JumpCheck)) && Jumping == null)
        {
            g = StaticExtraGravity;
            if (RedBrush) g = RedBrushGravity;

            if (SlopeAngleDebug)
            {
                Debug.Log(GravityAtMin + (GravityAtMax - GravityAtMin) * (SlopeDownAngle - MinAngleSlide) / (MaxAngleslide - MinAngleSlide));
            }

            if (SlopeDownAngle < MinAngleSlide)
            {
                rb.AddForce(Physics.gravity * StaticExtraGravity);
            }
            else if (SlopeDownAngle > MaxAngleslide)
            {
                rb.AddForce(Physics.gravity * (GravityAtMax + g));
            }
            else
            {
                rb.AddForce(Physics.gravity * (GravityAtMin + (GravityAtMax - GravityAtMin) * (SlopeDownAngle - MinAngleSlide) / (MaxAngleslide - MinAngleSlide) + g));
            }
        }
        else if (Jumping == null && MinJumpTime == null)
        {
            if (rb.linearVelocity.y > (Physics.gravity * MaxGravity).y)
            {
                if (Grounded) rb.AddForce(Physics.gravity * (Gravity + g));
                else rb.AddForce(Physics.gravity * Gravity);
            }
        }
        else
        {
            if (rb.linearVelocity.y > (Physics.gravity * MaxJumpingGravity).y)
            {
                rb.AddForce(Physics.gravity * JumpingGravity);
            }
        }
        
    }

    #region GroundChecking
    void V_GroundCheck()
    {
        Vector2 checkPos = new Vector3(composite2D.bounds.min.x, composite2D.bounds.min.y - GroundDownRange);

        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.right, (composite2D.bounds.max.x - composite2D.bounds.min.x), mask);

        if (!hit)
        {
            checkPos = new Vector3(composite2D.bounds.min.x, composite2D.bounds.min.y);

            hit = Physics2D.Raycast(checkPos, Vector2.down, GroundDownRange, mask);
        }

        if (hit)
        {
            if (hit.collider != null)
            {
                if (hit.collider.transform.parent != null)
                {
                    if (hit.transform.CompareTag("BlueBrush"))
                    {
                        Debug.DrawRay(hit.point, hit.normal, Color.orange);

                        RedBrush = true;
                        Grounded = true;
                        OnGreen = false;

                    }
                    else if (!hit.transform.CompareTag("GreenBrush"))
                    {
                        Debug.DrawRay(hit.point, hit.normal, Color.orange);

                        Grounded = true;
                        RedBrush = false;
                        OnGreen = false;

                    }
                    else if (hit.transform.CompareTag("GreenBrush"))
                    {
                        Debug.DrawRay(hit.point, hit.normal, Color.orange);
                        Debug.Log("Hit Green");
                        Grounded = false;
                        OnGreen = true;
                        RedBrush = false;
                    }
                }
                else
                {
                    Debug.DrawRay(hit.point, hit.normal, Color.orange);

                    RedBrush = false;
                    OnGreen = false;
                    Grounded = true;
                }
            }
            else
            {
                Debug.DrawRay(hit.point, hit.normal, Color.orange);

                RedBrush = false;
                OnGreen = false;
                Grounded = true;
            }

        }
        else
        {
            RedBrush = false;
            OnGreen = false;
            RedBrush = false;
            Grounded = false;
        }


        //This is kind of evil and should be changed to not run every update
        if (Grounded || OnSlope)
        {
            if(InAir != null)
            {
                StopCoroutine(InAir);
                InAir = null;
            }
            animator.SetBool("InAir_Bool", false);
            animator.SetTrigger("Landing_Trigger");
            AudioRunner();
        }
        else if(InAir == null)
        {
            InAir = StartCoroutine(C_InAir());
            AudioRunner();
        }

    }
    #endregion

    #region Audio
    IEnumerator Walk()
    {
        WalkPlayer.clip = SFX[2];
        WalkPlayer.pitch = Random.Range(0.8f, 1.2f);
        WalkPlayer.Play();
        yield return new WaitForSeconds(WalkSFXSpeed);
        NOTWalk = true;

    }

    IEnumerator C_InAir()
    {
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("InAir_Bool", true);
        AudioRunner();
        yield return new WaitForEndOfFrame();
    }
    void AudioRunner() // the shit determining what shit would be running
    {

        //AudioPlayer.Stop();
        if (animator.GetBool("InAir_Bool") && NOTJump)
            {
            SFXPlayer.Stop();
            SFXPlayer.clip = SFX[0];
            SFXPlayer.Play();
                NOTJump = false;
            }
            if (!animator.GetBool("InAir_Bool") && !NOTJump)
            {
            SFXPlayer.Stop();
            SFXPlayer.clip = SFX[1];
            SFXPlayer.Play();
                NOTJump = true;
            }
           else if (animator.GetBool("Walking") && Grounded && NOTJump && NOTWalk)
           {
                StartCoroutine(Walk());
                NOTWalk = false;
           }
    }

    

    #endregion

    #region SlopeChecking
    void SlopeCheck()
    {
        Vector2 checkPos = new Vector3(transform.position.x, composite2D.bounds.min.y);

        Vector2 checkPos1 = new Vector3(composite2D.bounds.max.x, composite2D.bounds.min.y + 0.1f);
        Vector2 checkPos2 = new Vector3(composite2D.bounds.min.x, composite2D.bounds.min.y + 0.1f);
        Vector2 checkPos3 = new Vector3((composite2D.bounds.min.x + composite2D.bounds.max.x)/2, composite2D.bounds.min.y + 0.1f);


        SlopeCheckVertical(checkPos1, checkPos2, checkPos3);
        SlopeCheckHorizontal(checkPos);
    }

    void SlopeCheckHorizontal(Vector2 checkPos)
    {
        checkPos = new Vector2(checkPos.x, (checkPos.y+0.2f));
        RaycastHit2D hit = Physics2D.Raycast(checkPos, new Vector2(Mathf.Round(Movement.x),0f), SlopeForwardRange, mask);

        if (hit)
        {
            Debug.DrawRay(hit.point, hit.normal, Color.red);

            SlopeForwardAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (SlopeForwardAngle < 90)
                LookingAtSlope = true;
            else
                LookingAtSlope = false;

            if (SlopeAngleDebug)
                Debug.Log(SlopeForwardAngle);
        }
        else
        {
            LookingAtSlope = false;
        }

    }

    void SlopeCheckVertical(Vector2 checkPos1, Vector2 checkPos2, Vector2 checkPos3)
    {
        RaycastHit2D hit1 = Physics2D.Raycast(checkPos1, Vector2.down, SlopeDownRange, mask);
        RaycastHit2D hit2 = Physics2D.Raycast(checkPos2, Vector2.down, SlopeDownRange, mask);
        RaycastHit2D hit3 = Physics2D.Raycast(checkPos3, Vector2.down, SlopeDownRange, mask);
        Debug.DrawRay(hit1.point, hit1.normal, Color.green);
        Debug.DrawRay(hit2.point, hit2.normal, Color.green);
        //Debug.DrawRay(hit3.point, hit3.normal, Color.green);

        float SlopeDownAngle1 = 0;
        float SlopeDownAngle2 = 0;
        float SlopeDownAngle3 = 0;


        if (hit1)
        {
            Vector2 SlopeNormalPerp1 = Vector2.Perpendicular(hit1.normal);

            SlopeDownAngle1 = Vector2.Angle(hit1.normal, Vector2.up);

            Debug.DrawRay(hit1.point, SlopeNormalPerp1, Color.yellow);
        }
        if (hit2)
        {
            Vector2 SlopeNormalPerp2 = Vector2.Perpendicular(hit2.normal);

            SlopeDownAngle2 = Vector2.Angle(hit2.normal, Vector2.up);

            Debug.DrawRay(hit2.point, SlopeNormalPerp2, Color.yellow);
        }
        if (hit3)
        {
            Vector2 SlopeNormalPerp3 = Vector2.Perpendicular(hit3.normal);

            SlopeDownAngle3 = Vector2.Angle(hit2.normal, Vector2.up);

            //Debug.DrawRay(hit3.point, SlopeNormalPerp3, Color.yellow);
        }

        SlopeDownAngle = Mathf.Max(SlopeDownAngle1, SlopeDownAngle2, SlopeDownAngle3);

        if (SlopeDownAngle > 0)
            OnSlope = true;
        else OnSlope = false;

        if (SlopeAngleDebug)
        {
            Debug.Log("Slope Down Angle: "+SlopeDownAngle);
            Debug.Log(rb.sharedMaterial.friction);
        }

    }
    #endregion

    #region Movement
    void GroundMove()
    {
        float maxSpeed = MaxSpeed;
        float accel = TrueGroundAcceleration;

        if (RedBrush)
        {
            maxSpeed *= RedBrushMaxspeedMod;
            accel *= RedBrushAccelerationMod;
        }

        if (Movement.magnitude > 0)
        {
            if ((rb.linearVelocity.x < 0 && Movement.x > 0) || (rb.linearVelocity.x > 0 && Movement.x < 0))
            {
                float i = Movement.x * accel;
                if (BonusTurnaroundSpeed >= 0)
                {
                    i -= rb.linearVelocity.x * BonusTurnaroundSpeed;
                }
                else
                {
                    i -= Movement.x * accel * Mathf.Abs(BonusTurnaroundSpeed);
                }
                rb.linearVelocity += new Vector2(i, 0);
            }
            else if (Mathf.Abs(rb.linearVelocity.x + Movement.x * accel) > maxSpeed
             && Mathf.Abs(rb.linearVelocity.x + Movement.x * accel) > Mathf.Abs(rb.linearVelocity.x))
            {
                float f = maxSpeed - Mathf.Abs(rb.linearVelocity.x);
                if (Mathf.Abs(rb.linearVelocity.x) < MaxSpeed) 
                    rb.linearVelocity += new Vector2(f * Movement.x, 0);
            }
            else rb.linearVelocity += new Vector2(Movement.x * accel, 0);
        }
        else if (rb.linearVelocity.x != 0)
        {
            if (rb.linearVelocity.x > 0)
            {
                if(Mathf.Abs(rb.linearVelocity.x) - TrueGroundDecceleration < 0)
                    rb.linearVelocity -= new Vector2(rb.linearVelocity.x, 0);
                else rb.linearVelocity -= new Vector2(TrueGroundDecceleration, 0);

            }
            else if (rb.linearVelocity.x < 0)
            {
                if (Mathf.Abs(rb.linearVelocity.x) - TrueGroundDecceleration < 0)
                    rb.linearVelocity -= new Vector2(rb.linearVelocity.x, 0);
                else rb.linearVelocity += new Vector2(TrueGroundDecceleration, 0);
            }
        }
    }

    void AirMove()
    {
        float maxSpeed = MaxSpeed;
        float accel = TrueAirAcceleration;

        if (RedBrush)
        {
            maxSpeed *= RedBrushMaxspeedMod;
            accel *= RedBrushAccelerationMod;
        }

        if (Movement.magnitude > 0)
        {
            if ((rb.linearVelocity.x < 0 && Movement.x > 0) || (rb.linearVelocity.x > 0 && Movement.x < 0))
            {
                rb.linearVelocity += new Vector2(Movement.x * accel - rb.linearVelocity.x * BonusTurnaroundSpeed, 0);
            }
            else if (Mathf.Abs(rb.linearVelocity.x + Movement.x * accel) > maxSpeed
                  && Mathf.Abs(rb.linearVelocity.x + Movement.x * accel) > Mathf.Abs(rb.linearVelocity.x))
            {
                float f = maxSpeed - Mathf.Abs(rb.linearVelocity.x);
                if (Mathf.Abs(rb.linearVelocity.x) < MaxSpeed)
                    rb.linearVelocity += new Vector2(f * Movement.x, 0);
            }
            else
                rb.linearVelocity += new Vector2(Movement.x * accel, 0);
        }
        else if (rb.linearVelocity.x != 0)
        {
            if (rb.linearVelocity.x > 0)
            {
                if (Mathf.Abs(rb.linearVelocity.x) - TrueAirDecceleration < 0)
                    rb.linearVelocity -= new Vector2(rb.linearVelocity.x, 0);
                else rb.linearVelocity -= new Vector2(TrueAirDecceleration, 0);

            }
            else if (rb.linearVelocity.x < 0)
            {
                if (Mathf.Abs(rb.linearVelocity.x) -TrueAirDecceleration < 0)
                    rb.linearVelocity -= new Vector2(rb.linearVelocity.x, 0);
                else rb.linearVelocity += new Vector2(TrueAirDecceleration, 0);
            }
        }
    }
    #endregion

    #region Jumping
    void V_Jumping()
    {
        if (rb.linearVelocity.y > 0 && !Grounded && Coyote == null)
            Coyote = StartCoroutine(CoyoteTimer(CoyoteTime));

        if (JumpAction.WasPressedThisFrame())
        {
            if (JumpBuffer != null)
            {
                StopCoroutine(JumpBuffer);
                JumpBuffer = StartCoroutine(C_JumpBuffer(JumpBufferTimer));
            }
            JumpBuffer = StartCoroutine(C_JumpBuffer(JumpBufferTimer));
        }

        //Jumps and adds an internal cooldown to how frequently the player can jump as otherwise the player gets to double jump due to how lenient the capsulecast is.
        if (JumpBuffer != null && JumpCheck && !JumpCooldownBool)
        {
            if (Jumping == null && ((SlopeDownAngle < MaxJumpAngle || (rb.linearVelocity.y > -0.5 && rb.linearVelocity.y <= 0)) || Coyote != null))
            {
                animator.SetTrigger("Jump_Trigger");
                Jumping = StartCoroutine(C_Jumping(MaximumJumpDuration));
                MinJumpTime = StartCoroutine(C_MinJumpDuration(MinimumJumpDuration));
                JumpCheck = false;
            }
            JumpCooldownBool = true;
            StartCoroutine(C_JumpCooldown(JumpCooldown));
        }


        if (JumpAction.WasReleasedThisFrame())
        {
            if (Jumping != null)
            {
                StopCoroutine(Jumping);
                Jumping = null;
            }
        }

        if (Grounded && !OnGreen)
        {
            JumpCheck = true;
        }
        else
        {
            if (Coyote == null && Jumping == null && !OnGreen)
            {
                Coyote = StartCoroutine(CoyoteTimer(CoyoteTime));
            }
        }
    }

    IEnumerator C_JumpCooldown(float timer)
    {
        yield return new WaitForSeconds(timer);
        JumpCheck = false;
        JumpCooldownBool = false;
        yield return new WaitForEndOfFrame();
    }

    IEnumerator CoyoteTimer(float timer)
    {
        yield return new WaitForSeconds(timer);
        JumpCheck = false;
        Coyote = null;
        yield return new WaitForEndOfFrame();
    }

    IEnumerator C_Jumping(float timer)
    {
        if (JumpBuffer != null)
        {
            StopCoroutine(JumpBuffer);
            JumpBuffer = null;
        }
        //IF THE LEVELS ARE BROKEN BECAUSE OF THIS SET IT BACK TO < 0

        if (rb.linearVelocity.y != 0 && !(InconsistentJumpsOnRedSlopes && rb.linearVelocity.y > 0 && RedBrush))
        {
            rb.linearVelocity -= new Vector2(0, rb.linearVelocity.y);
            Debug.Log(InconsistentJumpsOnRedSlopes + " " + (rb.linearVelocity.y < 0) + " " + RedBrush);
        }
        rb.AddForce(Vector2.up * TrueJumpHeight, ForceMode2D.Impulse);
        yield return new WaitForSeconds(timer);
        if (Jumping != null) Jumping = null;
        yield return new WaitForEndOfFrame();
    }

    IEnumerator C_MinJumpDuration(float timer)
    {
        yield return new WaitForSeconds(timer);
        MinJumpTime = null;
        if(!JumpAction.IsPressed() && Jumping != null)
        {
            StopCoroutine(Jumping);
            Jumping = null;
        }
    }

    IEnumerator C_JumpBuffer(float timer)
    {
        yield return new WaitForSeconds(timer);
        JumpBuffer = null;
        yield return new WaitForEndOfFrame();
    }
    #endregion

    #region ExperimentalFeatures
    void Experimental()
    {
        if (StopYMovementOnSlopes) StopYOnSlopes();
    }

    void StopYOnSlopes()
    {
        if (Movement.magnitude == 0 && (OnSlope || LookingAtSlope) && Jumping != null && !OnGreen)
        {
            if(rb.linearVelocity.y > 0) rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        }
    }
    #endregion
}