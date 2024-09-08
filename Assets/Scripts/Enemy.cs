using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

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
    private float cooldown = 0f;
    private bool isDying = false;

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

        if(Input.GetKeyDown(KeyCode.W))
        {
            Die();
        }

    }

    public virtual void Move()
    {
        Vector3 playerDir = isDying ? new Vector3(Player.Instance.transform.position.x, Player.Instance.transform.position.y - 500f, Player.Instance.transform.position.z) - transform.position : (Player.Instance.transform.position - transform.position).normalized;
        transform.position += transform.forward * speed * /*EnemyManager.Instance.Waves[EnemyManager.Instance.CurrentWave].DifficultyMultiplier * */Time.deltaTime;
        transform.forward = Vector3.Lerp(transform.forward, shouldPitch || isDying ? playerDir : Vector3.ProjectOnPlane(playerDir, Vector3.up), trackingSpeed * /*EnemyManager.Instance.Waves[EnemyManager.Instance.CurrentWave].DifficultyMultiplier * */Time.deltaTime);

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
        FXManager.Instance.PlaySFX("Explosion", 1f);
        GameManager.Instance.enemiesKilled++;
        isDying = true;
        FXManager.Instance.PlayVFX("Explosion", transform.position, 12.5f);
        GetComponentInChildren<VisualEffect>().enabled = true;
        EnemyManager.Instance.SpawnedEnemies.Remove(this);
        Invoke("Delete", 10f);
    }

    private void Delete()
    {
        Destroy(gameObject);
    }
}
