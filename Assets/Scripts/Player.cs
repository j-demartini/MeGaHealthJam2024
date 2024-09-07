using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] private float speed = 50f;
    [SerializeField] private float pitchSpeed = 50f;
    [SerializeField] private float yawSpeed = 25f;
    [SerializeField] private float rollSpeed = 50f;
    [SerializeField] private float noiseThreshold = 0.1f;
    [SerializeField] private float resetMultiplier = 1.5f;
    [Space]
    [SerializeField] private bool debug = false;

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
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
    }

    private void Movement()
    {
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

    private void Gun()
    {
        if (debug)
        {
            if (Input.GetKey(KeyCode.Space))
            {

            }
            //HardwareManager.Instance.HardwareObjects[(int)Sensor.LeftGun].Direction = new Vector3(0, 1, 0);
            //HardwareManager.Instance.HardwareObjects[(int)Sensor.RightGun].Direction = new Vector3(0, 1, 0);
        }
    }
}
