using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public PlayerAimAssist PlayerAimAssist { get => aimAssist; }

    [SerializeField] private float noiseThreshold = 0.1f;
    [Header("Movement")]
    [SerializeField] private float speed = 50f;
    [SerializeField] private float pitchSpeed = 50f;
    [SerializeField] private float yawSpeed = 25f;
    [SerializeField] private float rollSpeed = 50f;
    [SerializeField] private float resetMultiplier = 1.5f;
    [Header("Gun")]
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform[] bulletSpawnLocs;
    [SerializeField] private float bulletSpeed = 1000f;
    private float cooldown = 0f;

    [Header("Health")]
    [SerializeField] private int maxHitPoints = 20;
    [SerializeField] private float gracePeriod = 2f;
    private int maxHitPointsCache;
    private float graceTimer = 0f;

    [Header("Aim Assist")]
    [SerializeField] private PlayerAimAssist aimAssist;
    [SerializeField] private float aimAssistStrength = 2f;
    [SerializeField] private float bulletCurve = 2f;

    [Space]
    [SerializeField] private bool debug = false;

    [SerializeField] private AudioSource fireSource;

    private bool autoFire = false;

    void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        maxHitPointsCache = maxHitPoints;
    }

    // Update is called once per frame
    void Update()
    {
        if (debug || !GameManager.Instance.GameStarted)
        {
           return;
        }

        if (debug)
        {
            HardwareManager.Instance.HardwareObjects[(int)Sensor.LeftWheel].Direction = new Vector3(0, 0, 0);
            if (Input.GetKey(KeyCode.W))
            {
                HardwareManager.Instance.HardwareObjects[(int)Sensor.LeftWheel].Direction = new Vector3(0, 1, 0);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                HardwareManager.Instance.HardwareObjects[(int)Sensor.LeftWheel].Direction = new Vector3(0, -1, 0);
            }

            HardwareManager.Instance.HardwareObjects[(int)Sensor.RightWheel].Direction = new Vector3(0, 0, 0);
            if (Input.GetKey(KeyCode.UpArrow))
            {
                HardwareManager.Instance.HardwareObjects[(int)Sensor.RightWheel].Direction = new Vector3(0, 1, 0);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                HardwareManager.Instance.HardwareObjects[(int)Sensor.RightWheel].Direction = new Vector3(0, -1, 0);
            }
        }

        AimAssist();
        Movement();
        Gun();
        GracePeriod();
    }

    private void GracePeriod()
    {
        graceTimer += Time.deltaTime;
        if (maxHitPoints < maxHitPointsCache && graceTimer >= gracePeriod)
        {
            graceTimer = 0f;
            maxHitPoints += 1;
        }
    }

    private void Movement()
    {
        if (!GameManager.Instance.GameStarted)
        {
            return;
        }

        transform.position += transform.forward * speed * Time.deltaTime;
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(new Vector3(0, transform.localEulerAngles.y, 0)), resetMultiplier * Time.deltaTime);

        Vector3 leftDir = HardwareManager.Instance.HardwareObjects[(int)Sensor.LeftWheel].Direction;
        Vector3 rightDir = HardwareManager.Instance.HardwareObjects[(int)Sensor.RightWheel].Direction;

        if (leftDir.magnitude > noiseThreshold && rightDir.magnitude > noiseThreshold)
        {
            bool leftForward = leftDir.y > 0;
            bool rightForward = rightDir.y > 0;

            // Pitching
            if (leftForward == rightForward)
            {
                transform.Rotate(Vector3.right, pitchSpeed * Mathf.Sign(leftDir.y) * ((Mathf.Abs(leftDir.y) + Mathf.Abs(rightDir.y)) / 2) * Time.deltaTime);
            }
            // Yawing
            else
            {
                transform.Rotate(Vector3.up, yawSpeed * Mathf.Sign(leftDir.y) * ((Mathf.Abs(leftDir.y) + Mathf.Abs(rightDir.y)) / 2) * Time.deltaTime);
                transform.Rotate(Vector3.forward, rollSpeed * Mathf.Sign(-leftDir.y) * ((Mathf.Abs(leftDir.y) + Mathf.Abs(rightDir.y)) / 2) * Time.deltaTime);
            }
        }
    }

    private void AimAssist()
    {
        if (aimAssist.CurrentTarget != null)
        {
            if (aimAssist.CurrentTarget.GetComponent<Enemy>().isDying)
            {
                return;
            }
            Vector3 targetDir = (aimAssist.CurrentTarget.transform.position - transform.position).normalized;
            Vector3 cachedRot = transform.rotation.eulerAngles;
            transform.forward = Vector3.Lerp(transform.forward, targetDir, aimAssistStrength * Time.deltaTime);
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, cachedRot.z));
        }
    }

    private void Gun()
    {
        if (debug)
        {
            HardwareManager.Instance.HardwareObjects[(int)Sensor.Leg].Direction = new Vector3(0, 0, 0);
            if (Input.GetKey(KeyCode.Space))
            {
                HardwareManager.Instance.HardwareObjects[(int)Sensor.Leg].Direction = new Vector3(0, 1, 0);
            }
        }

        if(HardwareManager.Instance.HardwareObjects.Count < 3) 
        {
            autoFire = true;
        }


        cooldown += Time.deltaTime * (autoFire ? 1f :HardwareManager.Instance.HardwareObjects[(int)Sensor.Leg].Direction.magnitude);
        if ((autoFire || HardwareManager.Instance.HardwareObjects[(int)Sensor.Leg].Direction.magnitude > noiseThreshold) && cooldown >= fireRate)
        {
            cooldown = 0f;
            int gun = Random.Range(0, bulletSpawnLocs.Length);
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnLocs[gun].position, Quaternion.identity);
            bullet.transform.forward = transform.forward;
            bullet.GetComponent<Bullet>().FiredFrom = "Player";
            if (aimAssist.CurrentTarget != null)
            {
                bullet.GetComponent<Bullet>().AimAssistTarget = aimAssist.CurrentTarget;
                bullet.GetComponent<Bullet>().AimAssistStrength = bulletCurve;
            }
            bullet.GetComponent<Bullet>().Fire(bulletSpeed);
            StartCoroutine(MuzzleFlash(bulletSpawnLocs[gun].transform.GetChild(0).gameObject));

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

    private IEnumerator MuzzleFlash(GameObject flash)
    {
        flash.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        flash.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        if (graceTimer < gracePeriod)
        {
            return;
        }

        maxHitPoints -= damage;
        if (maxHitPoints <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        // TODO: VFX
        Debug.Log("Player died");
        Destroy(gameObject);
        EnemyManager.Instance.GameComplete = true;
    }

    void OnCollisonEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Environment"))
        {
            Die();
        }
    }
}
