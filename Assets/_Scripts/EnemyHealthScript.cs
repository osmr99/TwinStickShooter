using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class EnemyHealthScript : MonoBehaviour
{
    PlayerShootScript playerShootScript;
    [Header("Material Types")]
    [SerializeField] MeshRenderer[] _mRender;
    public Material baseMat;
    public Material hitFlashMat;

    [Header("Enemy Health Count")]
    public int enemyHealth = 5;

    [Header("Explosion Effect")]
    public GameObject explosionEffect;

    GameManager gameManager;
    PlayerController playerController;
    bool isDead = false;


    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        playerShootScript = GameObject.FindAnyObjectByType<PlayerShootScript>();
        playerController = FindAnyObjectByType<PlayerController>();
        foreach(MeshRenderer m in _mRender)
        {
            m.material = baseMat;
        }
    }

    private void Update()
    {
        if(isDead)
        {
            EnemyDead();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "PlayerBullet")
        {
            Destroy(other.gameObject);
            StartCoroutine(TakeDamage());
        }
        if (other.gameObject.tag == "LargeBullet")
        {
            StartCoroutine(TakeDamage());
        }
        if(other.gameObject.tag == "Player")
        {
            if(playerController.canTakeDamage)
                playerController.Damaged();
        }
    }

    IEnumerator TakeDamage()
    {
        enemyHealth--;
        if(enemyHealth <= 0)
        {
            isDead = true;
        }
        else
        {
            foreach (MeshRenderer m in _mRender)
            {
                m.material = hitFlashMat;
            }

            yield return new WaitForSeconds(0.1f);

            foreach (MeshRenderer m in _mRender)
            {
                m.material = baseMat;
            }
        }
    }

    void EnemyDead()
    {
        gameManager._enemiesInPlay--;
        gameManager.EnemyKillCount();
        Instantiate(explosionEffect, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        SoundManager.Instance.PlaySound3D(gameManager.sounds[8], transform.position, 0.5f);
        HitStopManager.Instance.DoHitStop(0.3f, 0.4f);
        Destroy(gameObject);
    }
}
