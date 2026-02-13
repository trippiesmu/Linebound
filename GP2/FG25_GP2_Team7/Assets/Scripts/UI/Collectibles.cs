using System.Linq;
using TMPro;
using UnityEngine;

public class Collectibles : MonoBehaviour
{
    [SerializeField] GameObject[] CollectibleArray;
    TMP_Text textMeshPro;
    string StartText;
    int CollectedCollectibles = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    private void OnEnable()
    {
        textMeshPro = GetComponent<TMP_Text>();
        StartText = textMeshPro.text;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        CollectedCollectibles = CollectibleArray.Length;
        foreach (GameObject item in CollectibleArray)
        {
            if (item.activeSelf)
            {
                CollectedCollectibles--;
            }
        }
        textMeshPro.text = StartText + CollectedCollectibles.ToString() + "/" + CollectibleArray.Length.ToString();
    }
}
