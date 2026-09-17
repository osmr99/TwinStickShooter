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
    public Rigidbody largeBullet;
    public float shotForce = 700;
    public int shotID = 1;
    public int spreadCount = 3;
    public float totalSpreadAngle = 30;

    [Header("Set Shoot Timing")]
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
        switch (shotID)
        {
            case 1:
                _shot = Instantiate(baseBullet, bulletSpawnPoint.position,
                    bulletSpawnPoint.rotation) as Rigidbody;
                _shot.AddForce(bulletSpawnPoint.forward * shotForce);
                break;
            case 2:
                _shot = Instantiate(largeBullet, bulletSpawnPoint.position,
                    bulletSpawnPoint.rotation) as Rigidbody;
                _shot.AddForce(bulletSpawnPoint.forward * shotForce * 0.75f);
                break;
            case 3:
                float angleStep = totalSpreadAngle / spreadCount - 1;
                float startAngle = -totalSpreadAngle / 2f;

                for(int i = 0; i < spreadCount; i++)
                {
                    float _angle = startAngle + angleStep * i;
                    Quaternion _rotation = Quaternion.AngleAxis(_angle, Vector3.up);

                    Vector3 _direction = _rotation * transform.forward;

                    _shot = Instantiate(baseBullet, bulletSpawnPoint.position,
                        Quaternion.LookRotation(_direction)) as Rigidbody;
                    _shot.AddForce(_direction * shotForce);
                }
                break;
        }

        yield return new WaitForSeconds(shootSpeed);
        canShoot = true;
    }
}
