using System.Collections;
using UnityEngine;

public class PlayerShootScript : MonoBehaviour
{
    TS_Inputs _inputs;
    PlayerController _ctrl;

    [Header("Spawn Setup")]
    public Transform bulletSpawnPoint;

    [Header("Bullet Types")]
    public Rigidbody baseBullet;
    public float shotForce = 700;

    [Header("Set Shoot Time")]
    public float shootSpeed = 0.2f;
    public bool canShoot;

    private void Awake()
    {
        _ctrl = GetComponent<PlayerController>();
        _inputs = new TS_Inputs();
        canShoot = true;
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
        if(_inputs.Player.Shoot.IsPressed() && canShoot)
            StartCoroutine(PlayerShoot());
    }

    IEnumerator PlayerShoot()
    {
        if (_ctrl.isGamepad) RumbleManager.Instance.RumblePulse(0.1f, 0.3f, 0.15f);
        canShoot = false;
        Rigidbody _shot;
        _shot = Instantiate(baseBullet, bulletSpawnPoint.position,
            bulletSpawnPoint.rotation) as Rigidbody;
        _shot.AddForce(bulletSpawnPoint.forward * shotForce);
        yield return new WaitForSeconds(shootSpeed);
        canShoot = true;
    }
}
