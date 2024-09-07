using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int HitPoints { get; private set; }

    [SerializeField] private int maxHitPoints = 2;
    [SerializeField] private float speed = 25f;
    [SerializeField] private float trackingSpeed = 1f;
    [SerializeField] private bool shouldPitch = true;
    [Space]
    [SerializeField] private float rollSpeed = 50f;

    // Start is called before the first frame update
    void Start()
    {
        HitPoints = maxHitPoints;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public virtual void Move()
    {
        Vector3 playerDir = (Player.Instance.transform.position - transform.position).normalized;
        transform.position += transform.forward * speed * EnemyManager.Instance.Waves[EnemyManager.Instance.CurrentWave].DifficultyMultiplier * Time.deltaTime;
        transform.forward = Vector3.Lerp(transform.forward, shouldPitch ? playerDir : Vector3.ProjectOnPlane(playerDir, Vector3.up), trackingSpeed * EnemyManager.Instance.Waves[EnemyManager.Instance.CurrentWave].DifficultyMultiplier * Time.deltaTime);

        //float angle = Vector3.SignedAngle(transform.forward, playerDir, Vector3.up);
        //transform.Rotate(transform.forward, rollSpeed * Mathf.Sign(angle) * Time.deltaTime);
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
        Destroy(gameObject);
    }
}
