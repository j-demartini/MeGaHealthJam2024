using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hardware : MonoBehaviour
{
    public Vector3 Direction { get; private set; }
    public float f;

    private Vector3 calibratedGyroPosition;
    private int calibratedPositions;
    private bool calibratedGyro;


    public void ReceiveGyro(float x, float y, float z)
    {
        Vector3 gyro = new Vector3(x, -z, -y);
        if(calibratedGyro)
        {
            transform.Rotate((gyro - (calibratedGyroPosition)) * (1f / 119f));
        } else
        {
            if(calibratedPositions == 100)
            {
                calibratedPositions = 0;
                calibratedGyro = true;
                calibratedGyroPosition /= 100;
            } else
            {
                calibratedGyroPosition += gyro;
                calibratedPositions++;
            }
        }
        Debug.Log("Received Gyro: " + gyro);
    }

    public void ReceiveAccel(float x, float y, float z)
    {
        Vector3 accel = new Vector3(x, y, z);
        Debug.Log("Received Accel: " + accel);
        //float Roll = Mathf.Atan2(y, z) * 180 / Mathf.PI;
        //float Pitch = Mathf.Atan2(-x, Mathf.Sqrt(y * y + z * z)) * 180 / Mathf.PI;
        //transform.rotation = Quaternion.Euler(Pitch, 0, Roll);
    }
}