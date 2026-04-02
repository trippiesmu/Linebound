using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hand : MonoBehaviour
{
    [SerializeField] GameObject crayon;
    [SerializeField] GameObject target;
    //[SerializeField] GameObject mouse;
    //[SerializeField] Camera gameCamera;
    [SerializeField] Draw draw;
    [SerializeField] Sprite circle;
    [SerializeField] Sprite cross;
    [SerializeField] Sprite redCrayon;
    [SerializeField] Sprite greenCrayon;
    [SerializeField] Sprite blueCrayon;
    [SerializeField] Sprite eraser;
    [SerializeField] float setCrayonScale = 1f;
    [SerializeField] float targetScale = 1f;
    [SerializeField] float eraserTargetMod = 2f;
    [SerializeField] float adjustHandX = 0.2f;
    [SerializeField] float adjustHandY = 1f;
    //[SerializeField] float adjustMouseX;
    //[SerializeField] float adjustMouseY;

    float scaleMod = 1f;
    
    Vector2 handPosition;
    Vector2 startingValues;
    Vector2 finalValues;
    Quaternion startingRotation;
    Quaternion rotatedRotation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        crayon = GameObject.FindGameObjectWithTag("Cursor");
        target = GameObject.FindGameObjectWithTag("Target");
        //mouse = GameObject.FindGameObjectWithTag("Mouse");
        //gameCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        draw = GameObject.FindGameObjectWithTag("Paint").GetComponent<Draw>();
        startingValues = new Vector2(adjustHandX, adjustHandY);
        finalValues = new Vector2 (adjustHandX-5f, adjustHandY-10f);
        startingRotation = new Quaternion(crayon.transform.rotation.x, crayon.transform.rotation.y, crayon.transform.rotation.z, crayon.transform.rotation.w);
        rotatedRotation = Quaternion.Euler(crayon.transform.rotation.eulerAngles.x+180, crayon.transform.rotation.eulerAngles.y, crayon.transform.rotation.eulerAngles.z);
        Debug.Log(startingRotation.eulerAngles);
        target.gameObject.GetComponent<RectTransform>().localScale = new Vector3(0.35f * targetScale, 0.35f * targetScale, 0.35f * targetScale);
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
        if (Pointer.current.press.IsPressed())
        {
            handPosition = (Pointer.current.position.ReadValue());
        }
        else
        {
            handPosition = (Mouse.current.position.ReadValue());
        }  
        crayon.transform.position = new Vector2(handPosition.x + (adjustHandX * scaleMod), handPosition.y + (adjustHandY * scaleMod));
        target.transform.position = handPosition;
        //mouse.transform.position = new Vector2(handPosition.x + adjustMouseX, handPosition.y + adjustMouseY);

        if (hit.collider == null || !hit.collider.CompareTag("Wall"))
        {
            crayon.GetComponent<UnityEngine.UI.Image>().enabled = true;
        }
        else
        {
            crayon.GetComponent<UnityEngine.UI.Image>().enabled = false;
        }

        if (hit.collider != null && hit.collider.CompareTag("NoDraw"))
        {
            target.GetComponent<UnityEngine.UI.Image>().sprite = cross;
        }
        else
        {
            target.GetComponent<UnityEngine.UI.Image>().sprite = circle;
        }

        if (draw.GetBrushIndex() == 0)
        {
            scaleMod = setCrayonScale;
            crayon.GetComponent<UnityEngine.UI.Image>().sprite = redCrayon;
            crayon.GetComponent<RectTransform>().localScale = new Vector3(2 * setCrayonScale, 2 * setCrayonScale, 2 * setCrayonScale);
            crayon.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 0);
            target.gameObject.GetComponent<RectTransform>().localScale = new Vector3(0.35f * targetScale, 0.35f * targetScale, 0.35f * targetScale);
            if (draw.GetCrayonAmount(0) <= 0)
            {
                crayon.GetComponent<UnityEngine.UI.Image>().enabled = false;
            }
            else
            {
                crayon.GetComponent<UnityEngine.UI.Image>().enabled = true;
            }
        }
        else if (draw.GetBrushIndex() == 1)
        {
            scaleMod = setCrayonScale;
            crayon.GetComponent<UnityEngine.UI.Image>().sprite = greenCrayon;
            crayon.GetComponent<RectTransform>().localScale = new Vector3(2 * setCrayonScale, 2 * setCrayonScale, 2 * setCrayonScale);
            crayon.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 0);
            target.gameObject.GetComponent<RectTransform>().localScale = new Vector3(0.35f * targetScale, 0.35f * targetScale, 0.35f * targetScale);
            if (draw.GetCrayonAmount(1) <= 0)
            {
                crayon.GetComponent<UnityEngine.UI.Image>().enabled = false;
            }
            else
            {
                crayon.GetComponent<UnityEngine.UI.Image>().enabled = true;
            }
        }
        else if(draw.GetBrushIndex() == 2)
        {
            scaleMod = setCrayonScale;
            crayon.GetComponent<UnityEngine.UI.Image>().sprite = blueCrayon;
            crayon.GetComponent<RectTransform>().localScale = new Vector3(2 * setCrayonScale, 2 * setCrayonScale, 2 * setCrayonScale);
            crayon.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 0);
            target.gameObject.GetComponent<RectTransform>().localScale = new Vector3(0.35f * targetScale, 0.35f * targetScale, 0.35f * targetScale);
            if (draw.GetCrayonAmount(2) <= 0)
            {
                crayon.GetComponent<UnityEngine.UI.Image>().enabled = false;
            }
            else
            {
                crayon.GetComponent<UnityEngine.UI.Image>().enabled = true;
            }
        }
        else if(draw.GetBrushIndex() == 3)
        {
            crayon.GetComponent<UnityEngine.UI.Image>().sprite = eraser;
            crayon.GetComponent<RectTransform>().localScale = new Vector3(setCrayonScale, setCrayonScale, setCrayonScale);
            crayon.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, -45);
            target.gameObject.GetComponent<RectTransform>().localScale = new Vector3(0.35f * targetScale * eraserTargetMod, 0.35f * targetScale * eraserTargetMod, 0.35f * targetScale * eraserTargetMod);
            scaleMod = setCrayonScale /2;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            adjustHandX = finalValues.x;
            adjustHandY = finalValues.y;
        }
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            adjustHandX = startingValues.x;
            adjustHandY = startingValues.y;
        }
    }
}
