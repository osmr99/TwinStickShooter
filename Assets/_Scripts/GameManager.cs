using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Spawn System")]
    public GameObject _enemy;
    public Transform[] _spawnPoints;
    public int _enemiesInPlay;
    public int _maxEnemyCount;
    public int enemyKills = 0;
    public float spawnSpeed = 2f;

    [Header("Panels")]
    public GameObject upgradePanel;
    public GameObject pausePanel;
    public GameObject gameOverPanel;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    private void Start()
    {
        InvokeRepeating("SpawnEnemy", 3f, spawnSpeed);
    }

    public void SpawnEnemy()
    {
        if(_enemiesInPlay < _maxEnemyCount)
        {
            _enemiesInPlay++;
            int _spawnChoice = Random.Range(0, _spawnPoints.Length);
            Debug.Log(_spawnChoice);

            Instantiate(_enemy, _spawnPoints[_spawnChoice].position,
                _spawnPoints[_spawnChoice].rotation);
        }
    }

    public void EnemyKillCount()
    {
        enemyKills++;
        if(enemyKills >= 5)
        {
            OpenUpgradePanel();
        }
    }
    public void OpenUpgradePanel()
    {
        Time.timeScale = 0;
        enemyKills = 0;
        upgradePanel.SetActive(true);
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

}
