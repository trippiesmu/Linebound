using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Erasor : MonoBehaviour
{
    [Tooltip("Only objects with this tag will be deletable. Leave empty to allow all.")]
    public string targetTag = "Brush";

    [Tooltip("Maximum raycast distance")]
    public float maxDistance = 100f;

    public GameObject hitObject;
    public Draw draw;
    public GameObject EraserSound;
    [SerializeField] private float eraseScale = 20f;

    List<Ray> rays;
    List<RaycastHit2D> hits;

    private void Awake()
    {
        EraserSound = GameObject.Instantiate(EraserSound, this.transform);
    }

    void Update()
    {
        // Check for mouse click
        /*if (Mouse.current.rightButton.isPressed && !Mouse.current.leftButton.isPressed)
        {
            TryDeleteObjectUnderCursor();
        }*/
    }

    public void TryDeleteObjectUnderCursor()
    {
        // Create a ray from the camera to the mouse position
        rays = new List<Ray>();
        hits = new List<RaycastHit2D>();
        bool alreadyHit = false;
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        rays.Add(ray);
        Ray ray2 = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue() + new Vector2(1 * eraseScale,0 * eraseScale));
        rays.Add(ray2);
        Ray ray3 = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue() + new Vector2(-1 * eraseScale, 0 * eraseScale));
        rays.Add(ray3);
        Ray ray4 = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue() + new Vector2(0 * eraseScale, 1 * eraseScale));
        rays.Add(ray4);
        Ray ray5 = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue() + new Vector2(0 * eraseScale, -1 * eraseScale));
        rays.Add(ray5);
        Ray ray6 = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue() + new Vector2(Mathf.Sqrt(2) / 2 * eraseScale, Mathf.Sqrt(2) / 2 * eraseScale));
        rays.Add(ray6);
        Ray ray7 = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue() + new Vector2(Mathf.Sqrt(2) / 2 * eraseScale, -Mathf.Sqrt(2) / 2 * eraseScale));
        rays.Add(ray7);
        Ray ray8 = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue() + new Vector2(-Mathf.Sqrt(2) / 2 * eraseScale, -Mathf.Sqrt(2) / 2 * eraseScale));
        rays.Add(ray8);
        Ray ray9 = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue() + new Vector2(-Mathf.Sqrt(2) / 2 * eraseScale, Mathf.Sqrt(2) / 2 * eraseScale));
        rays.Add(ray9);
        for (int i = 0;  i < rays.Count; i++)
        {
            hits.AddRange(Physics2D.GetRayIntersectionAll(rays[i], Mathf.Infinity));
            Debug.DrawRay(rays[i].origin, rays[i].direction * 100f, Color.yellow);
        }

        

        foreach (var hit in hits)
        {
            if (hit.collider.gameObject.CompareTag(targetTag) && alreadyHit == false)
            {
                alreadyHit = true;
                draw.AddCrayon(hit.transform.gameObject.GetComponentInParent<Brush>().GetIndex(), hit.collider.gameObject.GetComponentInParent<Brush>().GetLength());
                Destroy(hit.transform.gameObject);
                EraserSound.GetComponent<AudioSource>().Play();

            }
        }

        // Perform the raycast
        /*if (Physics.Raycast(ray, out hit))
        {
            hitObject = hit.collider.gameObject;

            // If a tag is specified, check it
            if (string.IsNullOrEmpty(targetTag) || hitObject.CompareTag(targetTag))
            {
                Debug.Log($"Deleting object: {hitObject.name}");
                Destroy(hitObject);
            }
            else
            {
                Debug.Log($"Object '{hitObject.name}' does not match tag '{targetTag}' and will not be deleted.");
            }
        }
        else
        {
            Debug.Log("No object under cursor.");
        }*/
    }
}