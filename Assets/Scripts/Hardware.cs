using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hardware : MonoBehaviour
{
    // Maximums
    public Vector3 MaxValues { get; set; }
    public Vector3 MinValues { get; set; }

    public Vector3 Direction { get; set; }
    public Vector3 Sum { get; set; }

    public int ID { get; private set; }

    // Calibration
    private Vector3 calibratedGyroPosition;
    private int calibratedPositions;
    private bool calibratedGyro;
    private float lastCommTime;


    private float hardwareTimeout = 3f;

    public void Init(int id)
    {
        ID = id;
        HardwareManager.Instance.TrackerConnected.Invoke(this);
        lastCommTime = Time.realtimeSinceStartup;
    }

    void Update()
    {
        if (Time.realtimeSinceStartup - lastCommTime > hardwareTimeout)
        {
            HardwareManager.Instance.TrackerDisconnected.Invoke(this);
            Destroy(this.gameObject);
        }
    }

    public void ReceiveGyro(float x, float y, float z)
    {
        Vector3 gyro = new Vector3(x, -z, -y);
        if (calibratedGyro)
        {
            Sum += (gyro - (calibratedGyroPosition)) * (1f / 119f);
            //transform.Rotate((gyro - (calibratedGyroPosition)) * (1f / 119f));
        }
        else
        {
            if (calibratedPositions == 100)
            {
                calibratedPositions = 0;
                calibratedGyro = true;
                calibratedGyroPosition /= 100;
            }
            else
            {
                calibratedGyroPosition += gyro;
                calibratedPositions++;
            }
        }
        Direction = (gyro - (calibratedGyroPosition)) * (1f / 119f);

        // I'm sorry
        // Max value
        if (Direction.x > MaxValues.x)
        {
            MaxValues = new Vector3(Direction.x, MaxValues.y, MaxValues.z);
        }
        if (Direction.y > MaxValues.y)
        {
            MaxValues = new Vector3(MaxValues.x, Direction.y, MaxValues.z);
        }
        if (Direction.z > MaxValues.z)
        {
            MaxValues = new Vector3(MaxValues.x, MaxValues.y, Direction.z);
        }

        // Min value
        if (Direction.x < MinValues.x)
        {
            MinValues = new Vector3(Direction.x, MinValues.y, MinValues.z);
        }
        if (Direction.y < MinValues.y)
        {
            MinValues = new Vector3(MinValues.x, Direction.y, MinValues.z);
        }
        if (Direction.z < MinValues.z)
        {
            MinValues = new Vector3(MinValues.x, MinValues.y, Direction.z);
        }

        lastCommTime = Time.realtimeSinceStartup;
    }

    public void ReceiveAccel(float x, float y, float z)
    {
        Vector3 accel = new Vector3(x, y, z);
        //float Roll = Mathf.Atan2(y, z) * 180 / Mathf.PI;
        //float Pitch = Mathf.Atan2(-x, Mathf.Sqrt(y * y + z * z)) * 180 / Mathf.PI;
        //transform.rotation = Quaternion.Euler(Pitch, 0, Roll);
        lastCommTime = Time.realtimeSinceStartup;
    }
}