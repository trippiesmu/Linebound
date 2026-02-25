using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Draw : MonoBehaviour
{
    [SerializeField] private Camera gameCamera;
    [Header("Input Red, Green, Blue")]
    [SerializeField] private GameObject[] brushes;
    [SerializeField] private GameObject colliderObject;
    [SerializeField] private Erasor erasor;

    //[SerializeField] private float crayonCap = 100f;
    //[SerializeField] private int platformCap = 5;
    //[SerializeField] private float remainingPlatforms;
    [SerializeField] private float remainingRedCrayon = 0f;
    [SerializeField] private float remainingGreenCrayon = 0f;
    [SerializeField] private float remainingBlueCrayon = 0f;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float volumeMod = 2f;
    [SerializeField] private float maxVolume = 0.8f;
    [SerializeField] private Material blueMat;
    [SerializeField] private Material greenMat;
    [SerializeField] private Material redMat;
    [Header("Switch for allowing moon jumping")]
    [SerializeField] private bool moonJump = true;
    [Header("Switch for allowing stacking jump pads")]
    [SerializeField] private bool stackJumpPad = false;

    LineRenderer currentLineRenderer;
    EdgeCollider2D currentCollider;
    private int brushIndex;
    private float tempRed;
    private float tempGreen;
    private float tempBlue;
    private List<(Vector2, Vector2)> rayArray = new List<(Vector2, Vector2)>();
    private float totalCrayonUsed = 0f;

    List<Vector2> points = new List<Vector2>();
    Vector2 lastPos;
    Vector2 firstPos;

    private void Start()
    {
        //remainingPlatforms = platformCap;
        //remainingInk = (inkCap / platformCap);
        gameCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        audioSource.clip = audioClip;
        audioSource.loop = true;
        audioSource.Play();
        audioSource.volume = 0f;
        brushIndex = 2;
    }

    private void Update()
    {
        Drawing();

        if(Keyboard.current.digit5Key.wasPressedThisFrame && !Mouse.current.leftButton.isPressed)
        {
            brushIndex = (brushIndex + 1) % brushes.Length;
            if (brushIndex == 0 && remainingRedCrayon <= 0)
            {
                brushIndex = (brushIndex + 1) % brushes.Length;
            }
            if (brushIndex == 1 && remainingGreenCrayon <= 0)
            {
                brushIndex = (brushIndex + 1) % brushes.Length;
            }
            if (brushIndex == 2 && remainingBlueCrayon <= 0)
            {
                brushIndex = (brushIndex + 1) % brushes.Length;
            }
        }
        if(Mouse.current.scroll.ReadValue().y > 0 && !Mouse.current.leftButton.isPressed)
        {
            if(brushIndex == 0)
            {
                brushIndex = brushes.Length;
            }
            brushIndex = (brushIndex-1)%brushes.Length;
            if (brushIndex == 2 && remainingBlueCrayon <= 0)
            {
                brushIndex = (brushIndex - 1) % brushes.Length;
            }
            if (brushIndex == 1 && remainingGreenCrayon <= 0)
            {
                brushIndex = (brushIndex - 1) % brushes.Length;
            }
            if (brushIndex == 0 && remainingRedCrayon <= 0)
            {
                brushIndex = brushes.Length;
                brushIndex = (brushIndex - 1) % brushes.Length;
            }
        }
        else if (Mouse.current.scroll.ReadValue().y < 0 && !Mouse.current.leftButton.isPressed)
        {
            brushIndex = (brushIndex + 1) % brushes.Length;
            if (brushIndex == 0 && remainingRedCrayon <= 0)
            {
                brushIndex = (brushIndex + 1) % brushes.Length;
            }
            if (brushIndex == 1 && remainingGreenCrayon <= 0)
            {
                brushIndex = (brushIndex + 1) % brushes.Length;
            }
            if (brushIndex == 2 && remainingBlueCrayon <= 0)
            {
                brushIndex = (brushIndex + 1) % brushes.Length;
            }
        }

        if (Keyboard.current.pageUpKey.wasPressedThisFrame && (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings-1))
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if (Keyboard.current.pageDownKey.wasPressedThisFrame && (SceneManager.GetActiveScene().buildIndex >= 1))
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }

    void Drawing()
    {
        if (currentLineRenderer != null && Mouse.current.rightButton.wasPressedThisFrame)
        {
            totalCrayonUsed -= currentLineRenderer.GetComponent<Brush>().GetLength();
            EndLine();
            erasor.TryDeleteObjectUnderCursor();
        }
        else
        {
            if (brushIndex != 3)
            {
                Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
                if (hit.collider == null || (!hit.collider.CompareTag("Wall") && !hit.collider.CompareTag("NoDraw") && !(brushIndex == 0 && hit.collider.CompareTag("NoDrawRed"))
                && !(brushIndex == 1 && hit.collider.CompareTag("NoDrawGreen")) && !(brushIndex == 2 && hit.collider.CompareTag("NoDrawBlue")) 
                && !(hit.collider.CompareTag("Brush") && brushIndex == 1 && stackJumpPad == false)))
                {
                    if (Mouse.current.leftButton.wasPressedThisFrame && (brushIndex == 0 && remainingRedCrayon > 0f || brushIndex == 1 && remainingGreenCrayon > 0f 
                        || brushIndex == 2 && remainingBlueCrayon > 0f))
                    {
                        tempRed = remainingRedCrayon;
                        tempGreen = remainingGreenCrayon;
                        tempBlue = remainingBlueCrayon;
                        points.Clear();
                        CreateBrush();
                    }
                    else if (Mouse.current.leftButton.isPressed && currentLineRenderer != null)
                    {
                        PointToMousePos();
                    }
                    else if (Mouse.current.leftButton.wasReleasedThisFrame && currentLineRenderer != null)
                    {
                        EndLine();
                        //remainingInk = (inkCap / platformCap);
                        //remainingPlatforms--;
                    }
                }
                else if (currentLineRenderer != null)
                {
                    EndLine();
                }
            }
            else
            {
                if (Mouse.current.leftButton.isPressed)
                {
                    erasor.TryDeleteObjectUnderCursor();
                }
            }
        }
    }

    private void EndLine()
    {
        if (brushIndex == 0)
        {
            currentLineRenderer.material = redMat;
        }
        else if (brushIndex == 1)
        {
            currentLineRenderer.material = greenMat;
            currentLineRenderer.GetComponent<BouncePad>().SetAngle(points[0], points[1]);
        }
        else if (brushIndex == 2)
        {
            currentLineRenderer.material = blueMat;
        }
        currentCollider.enabled = true;
        currentLineRenderer = null;
        audioSource.volume = 0f;
        rayArray = new List<(Vector2, Vector2)>();
        lastPos = gameCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }

    void CreateBrush()
    {
        GameObject brushInstance = Instantiate(brushes[brushIndex]);
        currentLineRenderer = brushInstance.GetComponent<LineRenderer>();
        currentCollider = Instantiate(colliderObject, brushInstance.transform).GetComponent<EdgeCollider2D>();

        //because you gotta have 2 points to start a line renderer, 
        Vector2 mousePos = gameCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        currentLineRenderer.SetPosition(0, mousePos);
        currentLineRenderer.SetPosition(1, mousePos);
        points.Add(mousePos);
        points.Add(new Vector2(mousePos.x+0.1f,mousePos.y));
        currentCollider.points = points.ToArray();
        firstPos = mousePos;
        lastPos = mousePos;
    }

    void AddAPoint(Vector2 pointPos)
    {
        if (brushes[brushIndex].GetComponent<Brush>().GetLine() == Brush.LineState.Curved)
        {
            points.Add(pointPos);
            currentLineRenderer.positionCount++;
            int positionIndex = currentLineRenderer.positionCount - 1;
            currentLineRenderer.SetPosition(positionIndex, pointPos);

            currentCollider.points = points.ToArray();
        }
        else if (brushes[brushIndex].GetComponent<Brush>().GetLine() == Brush.LineState.Straight)
        {
            currentLineRenderer.SetPosition(1, pointPos);
            points[1] = pointPos;
            currentCollider.points = points.ToArray();
        }
    }

    void PointToMousePos()
    {
        Vector2 mousePos = gameCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        if (brushes[brushIndex].GetComponent<Brush>().GetLine() == Brush.LineState.Curved)
        {
            if ((brushIndex == 0 && remainingRedCrayon > 0f) || (brushIndex == 1 && remainingGreenCrayon > 0f) || (brushIndex == 2 && remainingBlueCrayon > 0f))
            {
                if (rayArray.Count > 0)
                {
                    for (int i = 0; i < rayArray.Count; i++)
                    {
                        RaycastHit2D[] hits = Physics2D.LinecastAll(rayArray[i].Item1, rayArray[i].Item2);
                        foreach (var hit in hits)
                        {
                            if (hit.collider != null && (hit.collider.CompareTag("NoDraw") || (brushIndex == 0 && hit.collider.CompareTag("NoDrawRed")) 
                                || (brushIndex == 1 && hit.collider.CompareTag("NoDrawGreen")) || (brushIndex == 2 && hit.collider.CompareTag("NoDrawBlue")) 
                                || (hit.collider.CompareTag("Brush") && brushIndex == 1 && stackJumpPad == false)))
                            {
                                EndLine();
                            }
                        }

                    }

                }
                if (lastPos != mousePos && currentLineRenderer != null)
                {
                    if (brushIndex == 0)
                    {
                        remainingRedCrayon -= (Vector2.Distance(lastPos, mousePos) * brushes[brushIndex].GetComponent<Brush>().GetCost());
                    }
                    else if (brushIndex == 1)
                    {
                        remainingGreenCrayon -= (Vector2.Distance(lastPos, mousePos) * brushes[brushIndex].GetComponent<Brush>().GetCost());
                    }
                    else if (brushIndex == 2)
                    {
                        remainingBlueCrayon -= (Vector2.Distance(lastPos, mousePos) * brushes[brushIndex].GetComponent<Brush>().GetCost());
                    }
                    currentLineRenderer.GetComponent<Brush>().AddLength(Vector2.Distance(lastPos, mousePos) * brushes[brushIndex].GetComponent<Brush>().GetCost());
                    audioSource.volume = Mathf.Lerp(audioSource.volume, Vector2.Distance(lastPos, mousePos) * volumeMod, 0.05f);
                    audioSource.volume = Mathf.Clamp(audioSource.volume, 0, maxVolume);
                    AddAPoint(mousePos);
                    rayArray.Add((lastPos, mousePos));
                    lastPos = mousePos; 
                }
                else
                {
                    audioSource.volume = 0f;
                }
            }
            else
            {
                EndLine();
            }
        }

        else if (brushes[brushIndex].GetComponent<Brush>().GetLine() == Brush.LineState.Straight)
        {
            if ((brushIndex == 0 && remainingRedCrayon > 0f) || (brushIndex == 1 && remainingGreenCrayon > 0f) || (brushIndex == 2 && remainingBlueCrayon > 0f))
            {
                RaycastHit2D[] hits = Physics2D.LinecastAll(firstPos, mousePos);
                foreach (var hit in hits)
                {
                    if (hit.collider != null && (hit.collider.CompareTag("NoDraw") || (brushIndex == 0 && hit.collider.CompareTag("NoDrawRed")) 
                        || (brushIndex == 1 && hit.collider.CompareTag("NoDrawGreen")) || (brushIndex == 2 && hit.collider.CompareTag("NoDrawBlue")) 
                        || (hit.collider.CompareTag("Brush") && brushIndex == 1 && stackJumpPad == false)))
                    {
                        EndLine();
                    }
                }
                if (lastPos != mousePos && currentLineRenderer != null)
                {
                    if (brushIndex == 0)
                    {
                        remainingRedCrayon = tempRed - (Vector2.Distance(firstPos, mousePos) * brushes[brushIndex].GetComponent<Brush>().GetCost());
                    }
                    else if (brushIndex == 1)
                    {
                        remainingGreenCrayon = tempGreen - (Vector2.Distance(firstPos, mousePos) * brushes[brushIndex].GetComponent<Brush>().GetCost());
                    }
                    else if (brushIndex == 2)
                    {
                        remainingBlueCrayon = tempBlue - (Vector2.Distance(firstPos, mousePos) * brushes[brushIndex].GetComponent<Brush>().GetCost());
                    }
                    currentLineRenderer.GetComponent<Brush>().SetLength(Vector2.Distance(firstPos, mousePos) * brushes[brushIndex].GetComponent<Brush>().GetCost());
                    audioSource.volume = Mathf.Lerp(audioSource.volume, Vector2.Distance(lastPos, mousePos) * volumeMod, 0.05f);
                    audioSource.volume = Mathf.Clamp(audioSource.volume, 0, maxVolume);
                    AddAPoint(mousePos);
                    lastPos = mousePos;
                }
                else
                {
                    audioSource.volume = 0f;
                }
            }
            else
            {
                EndLine();
            }
        }
    }

    public void AddCrayon(int crayonColour, float crayonAmount)
    {
        /*if(remainingPlatforms < platformCap)
        {
            remainingPlatforms++;
        }*/
        if(remainingBlueCrayon <= 0 && remainingGreenCrayon <= 0 && remainingRedCrayon <= 0 && brushIndex != 3)
        {
            brushIndex = crayonColour;
        }
        if (crayonColour == 0)
        {
            remainingRedCrayon += crayonAmount;
        }
        else if (crayonColour == 1)
        {
            remainingGreenCrayon += crayonAmount;
        }
        else if (crayonColour == 2)
        {
            remainingBlueCrayon += crayonAmount;
        }

    }

    public void SelectBrush(int index)
    {
        brushIndex = index % brushes.Length;
    }

    public float GetCrayonAmount(int index)
    {
        float temp;
        if (index == 0)
        {
            temp = remainingRedCrayon;
        }
        else if (index == 1)
        {
            temp = remainingGreenCrayon;
        }
        else
        {
            temp = remainingBlueCrayon;
        }
        return temp;
    }

    public int GetBrushIndex()
    {
        return brushIndex;
    }

    public void AddTotalCrayon(float amount)
    {
        totalCrayonUsed += amount;
    }

    public float GetTotalCrayonUsed()
    {
        return totalCrayonUsed;
    }

    public bool GetMoonJumpBool()
    {
        return moonJump;
    }

}
