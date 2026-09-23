using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    TS_Inputs _inputs;
    CharacterController cc;

    [Header("Player Controls")]
    public int currentHealth;
    public int maxHealth;
    public float moveSpeed = 4f;
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private Vector2 aimInput;

    [Header("Gamepad Setup")]
    PlayerInput pInput;
    [SerializeField] float controllerDeadZone = 0.1f;
    [SerializeField] float controllerRotateSmooth = 1000;

    public bool isDead;
    public bool isGamepad;
    public bool canTakeDamage = true;
    public float InvTime;
    GameManager gameManager;
    PlayerStats playerStats;
    [SerializeField] GameObject pausedHUD;
    [SerializeField] GameObject powerupHUD;

    private void Awake()
    {
        _inputs = new TS_Inputs();
        cc = GetComponent<CharacterController>();
        gameManager = FindAnyObjectByType<GameManager>();
        playerStats = FindAnyObjectByType<PlayerStats>();
    }

    private void OnEnable()
    {
        _inputs.Enable();
    }

    private void OnDisable()
    {
        _inputs.Disable();
    }

    private void Update()
    {
        HandleInputs();
        if(!isDead)
        {
            HandleMovement();
            HandleRotation();
        }

        if (_inputs.Player.Pause.WasPressedThisFrame())
        {
            if(!powerupHUD.activeInHierarchy && !isDead)
            {
                if(!pausedHUD.activeInHierarchy)
                {
                    Time.timeScale = 0;
                    pausedHUD.SetActive(true);
                }
                else
                {
                    Time.timeScale = 1;
                    pausedHUD.SetActive(false);
                }
            }
        }
    }

    public void Damaged()
    {
        canTakeDamage = false;
        currentHealth--;
        playerStats.HealthChange();
        if(currentHealth == 0)
        {
            isDead = true;
            int rand = Random.Range(10, 12);
            SoundManager.Instance.PlaySound3D(gameManager.sounds[rand], transform.position, 0.5f);
            StartCoroutine(GameOver());
        }
        else
        {
            SoundManager.Instance.PlaySound3D(gameManager.sounds[9], transform.position, 0.5f);
            StartCoroutine(InvSeconds());
        }
    }

    IEnumerator GameOver()
    {
        Time.timeScale = 0;
        Destroy(gameManager.gameObject);
        float rand = Random.Range(1.5f, 3.5f);
        yield return new WaitForSecondsRealtime(rand);
        Time.timeScale = 1;
        SceneManager.LoadScene("GameOver");
    }

    IEnumerator InvSeconds()
    {
        yield return new WaitForSecondsRealtime(InvTime);
        canTakeDamage = true;
    }

    public void OnDeviceChange(PlayerInput ctrl)
    {
        isGamepad = ctrl.currentControlScheme.Equals("Gamepad") ? true : false;
    }

    void HandleInputs()
    {
        moveInput = _inputs.Player.Movement.ReadValue<Vector2>();
        aimInput = _inputs.Player.Aiming.ReadValue<Vector2>();
    }

    void HandleMovement()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        cc.Move(move * moveSpeed * Time.deltaTime);
    }

    void HandleRotation()
    {
        if(Time.timeScale != 0)
        {
            if (isGamepad)
            {
                if (Mathf.Abs(aimInput.x) > controllerDeadZone || Mathf.Abs(aimInput.y) > controllerDeadZone)
                {
                    Vector3 playerDirection = Vector3.right * aimInput.x
                        + Vector3.forward * aimInput.y;
                    if (playerDirection.sqrMagnitude > 0)
                    {
                        Quaternion newRotation = Quaternion.LookRotation(playerDirection, Vector3.up);
                        transform.rotation = Quaternion.RotateTowards(transform.rotation,
                            newRotation, controllerRotateSmooth * Time.deltaTime);
                    }
                }
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(aimInput);
                Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

                float rayDistance; // from our mouse position to our ground plane

                if (groundPlane.Raycast(ray, out rayDistance))
                {
                    Vector3 point = ray.GetPoint(rayDistance);
                    LookAt(point);
                }
            }
        }
    }

    void LookAt(Vector3 lookPoint)
    {
        if(Time.timeScale != 0)
        {
            Vector3 heightCorrectedPoint = new Vector3(lookPoint.x,
                transform.position.y, lookPoint.z);
            transform.LookAt(heightCorrectedPoint);
        }
    }
}
