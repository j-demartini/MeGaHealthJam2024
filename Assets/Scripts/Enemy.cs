using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int HitPoints { get; private set; }

    [Header("Enemy Stats")]
    [SerializeField] private int maxHitPoints = 2;
    [SerializeField] private float speed = 25f;
    [SerializeField] private float trackingSpeed = 1f;
    [SerializeField] private bool shouldPitch = true;
    [Space]
    [SerializeField] private float rollSpeed = 50f;

    [Header("Gun")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform[] bulletSpawnLoc;
    [SerializeField] private float bulletSpeed = 100f;
    [SerializeField] private float fireRate = 0.5f;
    private float cooldown = 0f;

    // Start is called before the first frame update
    void Start()
    {
        HitPoints = maxHitPoints;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Gun();
    }

    public virtual void Move()
    {
        Vector3 playerDir = (Player.Instance.transform.position - transform.position).normalized;
        transform.position += transform.forward * speed * EnemyManager.Instance.Waves[EnemyManager.Instance.CurrentWave].DifficultyMultiplier * Time.deltaTime;
        transform.forward = Vector3.Lerp(transform.forward, shouldPitch ? playerDir : Vector3.ProjectOnPlane(playerDir, Vector3.up), trackingSpeed * EnemyManager.Instance.Waves[EnemyManager.Instance.CurrentWave].DifficultyMultiplier * Time.deltaTime);

        //float angle = Vector3.SignedAngle(transform.forward, playerDir, Vector3.up);
        //transform.Rotate(transform.forward, rollSpeed * Mathf.Sign(angle) * Time.deltaTime);
    }

    public virtual void Gun()
    {
        Vector3 playerDir = (Player.Instance.transform.position - transform.position).normalized;
        cooldown += Time.deltaTime;
        if (Vector3.Angle(transform.forward, playerDir) < 25f && cooldown >= fireRate)
        {
            cooldown = 0f;
            foreach (Transform spawnLoc in bulletSpawnLoc)
            {
                GameObject bullet = Instantiate(bulletPrefab, spawnLoc.position, spawnLoc.rotation);
                bullet.transform.forward = transform.forward;
                bullet.GetComponent<Bullet>().FiredFrom = "Enemy";
                bullet.GetComponent<Bullet>().Fire(bulletSpeed);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        HitPoints -= damage;
        if (HitPoints <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        // TODO: VFX
        Debug.Log("Enemy died");
        Destroy(gameObject);
    }
}
