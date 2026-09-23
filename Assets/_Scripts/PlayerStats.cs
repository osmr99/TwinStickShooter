using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    PlayerController playerController;
    PlayerShootScript playerShootScript;
    [SerializeField] TMP_Text healthText;
    [SerializeField] TMP_Text spreadText;

    private void Awake()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        playerShootScript = FindAnyObjectByType<PlayerShootScript>();
    }

    private void Start()
    {
        healthText.text = "Health\n" + playerController.currentHealth + "/" + playerController.maxHealth;
        spreadText.text = "Max Spread\n" + playerShootScript.spreadCount;
    }

    public void HealthChange()
    {
        healthText.text = "Health\n" + playerController.currentHealth + "/" + playerController.maxHealth;
    }

    public void SpreadChange()
    {
        spreadText.text = "Max Spread\n" + playerShootScript.spreadCount;
    }
}
