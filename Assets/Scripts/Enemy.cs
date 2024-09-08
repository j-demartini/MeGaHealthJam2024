using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int HitPoints { get; private set; }
    public bool ShouldPitch { get => shouldPitch; }

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
    [SerializeField] private Transform[] fireSpawnLoc;
    [SerializeField] private float bulletSpeed = 100f;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private float fireRadius = 15f;
    [SerializeField] private AudioSource fireSource;
    private float cooldown = 0f;
    public bool isDying = false;

    public float explosionSize = 25f;

    // Start is called before the first frame update
    void Start()
    {
        HitPoints = maxHitPoints;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (!isDying)
            Gun();
    }

    public virtual void Move()
    {
        Vector3 playerDir = isDying ? new Vector3(Player.Instance.transform.position.x, Player.Instance.transform.position.y - 500f, Player.Instance.transform.position.z) - transform.position : (Player.Instance.transform.position - transform.position).normalized;
        transform.position += transform.forward * speed * /*EnemyManager.Instance.Waves[EnemyManager.Instance.CurrentWave].DifficultyMultiplier * */Time.deltaTime;
        transform.forward = Vector3.Lerp(transform.forward, shouldPitch ? playerDir : Vector3.ProjectOnPlane(playerDir, Vector3.up), trackingSpeed * /*EnemyManager.Instance.Waves[EnemyManager.Instance.CurrentWave].DifficultyMultiplier * */Time.deltaTime);

        //float angle = Vector3.SignedAngle(transform.forward, playerDir, Vector3.up);
        //transform.Rotate(transform.forward, rollSpeed * Mathf.Sign(angle) * Time.deltaTime);
    }

    public virtual void Gun()
    {
        Vector3 playerDir = (Player.Instance.transform.position - transform.position).normalized;
        cooldown += Time.deltaTime;
        if (Vector3.Angle(transform.forward, playerDir) < fireRadius && cooldown >= fireRate)
        {
            cooldown = 0f;
            foreach (Transform spawnLoc in bulletSpawnLoc)
            {
                GameObject bullet = Instantiate(bulletPrefab, spawnLoc.position, spawnLoc.rotation);
                bullet.transform.forward = transform.forward;
                bullet.GetComponent<Bullet>().FiredFrom = "Enemy";
                bullet.GetComponent<Bullet>().Fire(bulletSpeed);
            }

            if (!fireSource.isPlaying)
            {
                fireSource.Play();
            }
        }
        else if (fireSource.isPlaying)
        {
            fireSource.Stop();
        }
    }

    public void TakeDamage(int damage)
    {
        HitPoints -= damage;
        if (HitPoints <= 0 && !isDying)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        // TODO: VFX
        Debug.Log("Enemy died");
        GameManager.Instance.enemiesKilled++;
        isDying = true;
        FXManager.Instance.PlayVFX("Explosion", transform.position, explosionSize);
        foreach (Transform spawnLoc in fireSpawnLoc)
        {
            FXManager.Instance.PlayVFX("Fire", spawnLoc.position, 1f);
        }
        EnemyManager.Instance.SpawnedEnemies.Remove(this);
        Invoke("Delete", 10f);
    }

    private void Delete()
    {
        Destroy(gameObject);
    }
}
