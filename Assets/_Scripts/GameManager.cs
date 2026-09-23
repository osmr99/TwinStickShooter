using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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
    public float[] timesLeft = {10.15f, 10.15f, 10.15f, 10.15f};
    public GameObject firstButton;

    [Header("Audio Clips")]
    public AudioClip[] sounds;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    private void Start()
    {
        InvokeRepeating("SpawnEnemy", 0.5f, spawnSpeed);
        playerController = FindAnyObjectByType<PlayerController>();
        playerShootScript = FindAnyObjectByType<PlayerShootScript>();
        powerupUI = FindAnyObjectByType<PowerupUI>();
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
        Time.timeScale = 1.0f;
        upgradePanel.SetActive(false);
    }    

    public void PauseGame()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1.0f;
        pausePanel.SetActive(false);
    }

    public void UpgradeSelection(int choice)
    {
        switch (choice)
        {
            case 1:
                timesLeft[0] = 10.15f;
                playerController.moveSpeed += 4;
                RandomSFX();
                StartCoroutine(UpgradeOne());
                powerupUI.AddUI(choice);
                break;
            case 2:
                timesLeft[1] = 10.15f;
                playerShootScript.shotID = 2;
                RandomSFX();
                StopCoroutine(UpgradeTwo());
                StopCoroutine(UpgradeThree());
                StartCoroutine(UpgradeTwo());
                powerupUI.AddUI(choice);
                break;
            case 3:
                timesLeft[2] = 10.15f;
                playerShootScript.shotID = 3;
                RandomSFX();
                StopCoroutine(UpgradeTwo());
                StopCoroutine(UpgradeThree());
                StartCoroutine(UpgradeThree());
                powerupUI.AddUI(choice);
                break;
            case 4:
                timesLeft[3] = 10.15f;
                playerShootScript.shotForce += 700;
                RandomSFX();
                StartCoroutine(UpgradeFour());
                powerupUI.AddUI(choice);
                break;
            case 5:
                playerShootScript.spreadCount += spreadCountIncrease;
                SoundManager.Instance.PlaySound3D(sounds[5], playerController.transform.position, 0.4f);
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
        Debug.Log(randNum);
        float vol = 0;
        switch (randNum)
        {
            case 1:
                vol = 0.35f;
                break;
            case 2:
                vol = 0.225f;
                break;
            case 3:
                vol = 0.3f;
                break;
            case 4:
                vol = 0.25f;
                break;
        }
        SoundManager.Instance.PlaySound3D(sounds[randNum], playerController.transform.position, vol);
    }

    IEnumerator UpgradeOne()
    {
        if(Time.timeScale != 0)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            timesLeft[0] -= 0.1f;
            if(timesLeft[0] > 0)
            {
                StartCoroutine(UpgradeOne());
            }
            else
            {
                timesLeft[0] = 10.15f;
                playerController.moveSpeed -= 4;
            }
        }
        else
        {
            yield return new WaitForSecondsRealtime(0.01f);
            StartCoroutine(UpgradeOne());
        }
    }

    IEnumerator UpgradeTwo()
    {
        if (Time.timeScale != 0)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            timesLeft[1] -= 0.1f;
            if (timesLeft[1] > 0)
            {
                StartCoroutine(UpgradeTwo());
            }
            else
            {
                timesLeft[1] = 10.15f;
                playerShootScript.shotID = 1;
            }
        }
        else
        {
            yield return new WaitForSecondsRealtime(0.01f);
            StartCoroutine(UpgradeTwo());
        }
    }

    IEnumerator UpgradeThree()
    {
        if (Time.timeScale != 0)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            timesLeft[2] -= 0.1f;
            if (timesLeft[2] > 0)
            {
                StartCoroutine(UpgradeThree());
            }
            else
            {
                timesLeft[2] = 10.15f;
                playerShootScript.shotID = 1;
            }
        }
        else
        {
            yield return new WaitForSecondsRealtime(0.01f);
            StartCoroutine(UpgradeThree());
        }
    }

    IEnumerator UpgradeFour()
    {
        if (Time.timeScale != 0)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            timesLeft[1] -= 0.1f;
            if (timesLeft[1] > 0)
            {
                StartCoroutine(UpgradeFour());
            }
            else
            {
                timesLeft[3] = 10.15f;
                playerShootScript.shotForce -= 700;
            }
        }
        else
        {
            yield return new WaitForSecondsRealtime(0.01f);
            StartCoroutine(UpgradeFour());
        }
    }

}
