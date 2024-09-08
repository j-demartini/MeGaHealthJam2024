using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public string FiredFrom { get; set; }
    public float AimAssistStrength { get; set; }
    public GameObject AimAssistTarget { get; set; }

    [SerializeField] private int damage = 1;

    private Rigidbody rb;
    private bool fired = false;
    private float speed;

    // Update is called once per frame
    void Update()
    {
        if (fired)
        {
            rb.velocity = transform.forward * speed;

            if (AimAssistTarget != null)
            {
                Vector3 direction = (AimAssistTarget.transform.position - transform.position).normalized;
                transform.forward = Vector3.Lerp(transform.forward, direction, AimAssistStrength * Time.deltaTime);
            }
        }

        if (transform.position.magnitude > 1000)
        {
            Destroy(gameObject);
        }
    }

    public void Fire(float speed)
    {
        rb = GetComponent<Rigidbody>();
        float spread = Random.Range(-5f, 5f);
        transform.Rotate(transform.forward, spread);
        this.speed = speed;
        fired = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && FiredFrom == "Player")
        {
            Enemy enemy = other.gameObject.GetComponentInParent<Enemy>();
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Player") && FiredFrom == "Enemy")
        {
            Player.Instance.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
