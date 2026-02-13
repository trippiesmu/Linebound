using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonControler : MonoBehaviour
{
    List<Worlds> WorldSelectors = new List<Worlds>();

    class Worlds
    {
        public Button World;
        public List<Button> Levels = new List<Button>();

        public Worlds(Button world)
        {
            World = world;//.GetComponent<Button>();

            foreach(Button level in world.GetComponentsInChildren<Button>())
            {
                Levels.Add(level);
            }
            Levels.RemoveAt(0);
        }
    }

    private void Awake()
    {
        foreach (Button child in transform.GetComponentsInChildren<Button>())
        {
            WorldSelectors.Add(new Worlds(child));
        }
        foreach(Worlds world in WorldSelectors)
        {
            world.World.onClick.AddListener(delegate { WorldSelector(world); });
            foreach(Button level in world.Levels)
            {
                level.gameObject.SetActive(false);
            }
        }
    }

    private void OnDisable()
    {
        foreach (Worlds world in WorldSelectors)
        {
            world.World.onClick.RemoveAllListeners();
        }
    }

    void WorldSelector(Worlds world)
    {
        foreach(Worlds worlds in WorldSelectors)
        {
            foreach(Button level in worlds.Levels)
            {
                level.gameObject.SetActive(false);
            }
        }

        foreach(Button levels in world.Levels)
        {
            levels.gameObject.SetActive(true);
        }
    }
}
