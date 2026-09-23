using UnityEngine;
using System.Collections;

public class PowerupUI : MonoBehaviour
{
    //public List<GameObject> UIElements = new List<GameObject>();
    Transform powerupUI;
    public GameObject textPrefab;
    public int currentChoice;

    private void Start()
    {
        powerupUI = gameObject.transform;
    }

    public void AddUI(int choice)
    {
        currentChoice = choice;
        Instantiate(textPrefab, powerupUI);
    }
}
