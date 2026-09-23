using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    //public static GameManager Instance;

    [Header("Spawn System")]
    public GameObject _enemy;
    public Transform[] _spawnPoints;
    public int _enemiesInPlay;
    public int _maxEnemyCount;
    public int enemyKills;
    public float spawnSpeed;
    public int enemiesToKillForPowerup;
    public float powerupsDuration;

    [Header("Panels")]
    public GameObject upgradePanel;
    public GameObject pausePanel;
    public GameObject gameOverPanel;

    [Header("Upgrade References and Stuff")]
    public int spreadCountIncrease;
    public int maxHealthIncrease;
    PowerupUI powerupUI;
    PlayerController playerController;
    PlayerShootScript playerShootScript;
    public GameObject firstButton;
    float initialValue;
    float upgrade1, upgrade2, upgrade3, upgrade4;
    bool b1, b2, b3, b4;

    [Header("Audio Clips")]
    public AudioClip[] sounds;

    private void Awake()
    {
        //if(Instance == null)
            //Instance = this;
    }

    private void Start()
    {
        InvokeRepeating("SpawnEnemy", 0.5f, spawnSpeed);
        playerController = FindAnyObjectByType<PlayerController>();
        playerShootScript = FindAnyObjectByType<PlayerShootScript>();
        powerupUI = FindAnyObjectByType<PowerupUI>();
        initialValue = powerupsDuration + 0.15f;
        b1 = false;
        b2 = false;
        b3 = false;
        b4 = false;
    }

    public void SpawnEnemy()
    {
        if(_enemiesInPlay < _maxEnemyCount)
        {
            _enemiesInPlay++;
            int _spawnChoice = Random.Range(0, _spawnPoints.Length);

            Instantiate(_enemy, _spawnPoints[_spawnChoice].position,
                _spawnPoints[_spawnChoice].rotation);
        }
    }

    public void EnemyKillCount()
    {
        enemyKills++;
        if(enemyKills == enemiesToKillForPowerup)
        {
            OpenUpgradePanel();
        }
    }
    public void OpenUpgradePanel()
    {
        Time.timeScale = 0;
        enemyKills = 0;
        upgradePanel.SetActive(true);
        SoundManager.Instance.PlaySound3D(sounds[0], playerController.transform.position, 0.5f);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    public void CloseUpgradePanel()
    {
        Time.timeScale = 1;
        upgradePanel.SetActive(false);
    }    

    public void PauseGame()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    public void UpgradeSelection(int choice)
    {
        switch (choice)
        {
            case 1:
                playerController.moveSpeed += 2;
                RandomSFX();
                b1 = true;
                upgrade1 = powerupsDuration + 0.15f;
                powerupUI.AddUI(choice);
                break;
            case 2:
                playerShootScript.shotID = 2;
                RandomSFX();
                b2 = true;
                upgrade2 = powerupsDuration + 0.15f;
                powerupUI.AddUI(choice);
                break;
            case 3:
                playerShootScript.shotID = 3;
                RandomSFX();
                b3 = true;
                upgrade3 = powerupsDuration + 0.15f;
                powerupUI.AddUI(choice);
                break;
            case 4:
                playerShootScript.shotForce += 350;
                RandomSFX();
                b4 = true;
                upgrade4 = powerupsDuration + 0.15f;
                powerupUI.AddUI(choice);
                break;
            case 5:
                playerShootScript.spreadCount += spreadCountIncrease;
                SoundManager.Instance.PlaySound3D(sounds[5], playerController.transform.position, 0.3f);
                break;
            case 6:
                if(playerController.currentHealth == playerController.maxHealth)
                {
                    playerController.maxHealth += maxHealthIncrease;
                    playerController.currentHealth = playerController.maxHealth;
                    SoundManager.Instance.PlaySound3D(sounds[6], playerController.transform.position, 0.65f);
                }
                else
                {
                    playerController.currentHealth += maxHealthIncrease * 2;
                    if(playerController.currentHealth > playerController.maxHealth)
                    {
                        playerController.currentHealth = playerController.maxHealth;
                    }
                    SoundManager.Instance.PlaySound3D(sounds[7], playerController.transform.position, 0.5f);
                }
                break;
        }
    }

    void RandomSFX()
    {
        int randNum = Random.Range(1, 5);
        float vol = 0;
        switch (randNum)
        {
            case 1:
                vol = 0.25f;
                break;
            case 2:
                vol = 0.125f;
                break;
            case 3:
                vol = 0.2f;
                break;
            case 4:
                vol = 0.15f;
                break;
        }
        SoundManager.Instance.PlaySound3D(sounds[randNum], playerController.transform.position, vol);
    }

    private void FixedUpdate()
    {
        if(b1)
        {
            upgrade1 -= Time.deltaTime;
            if(upgrade1 < 0)
            {
                b1 = false;
            }
        }

        if (b2)
        {
            upgrade2 -= Time.deltaTime;
            if (upgrade2 < 0)
            {
                playerShootScript.shotID = 1;
                b2 = false;
            }
        }

        if (b3)
        {
            upgrade3 -= Time.deltaTime;
            if (upgrade3 < 0)
            {
                playerShootScript.shotID = 1;
                b3 = false;
            }
        }

        if (b4)
        {
            upgrade4 -= Time.deltaTime;
            if (upgrade4 < 0)
            {
                b4 = false;
            }
        }

        if (playerShootScript.shotID == 2)
        {
            b3 = false;
            upgrade3 = 0;
        }

        if (playerShootScript.shotID == 3)
        {
            b2 = false;
            upgrade2 = 0;
        }
    }

}
