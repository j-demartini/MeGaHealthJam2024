using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hardware : MonoBehaviour
{

    public float f;


    public void ReceiveGyro(float x, float y, float z)
    {
        Vector3 gyro = new Vector3(x, -z, -y);
        Debug.Log("Received Gyro: " + gyro);
        //transform.Rotate(gyro * (1f / 119f));
    }

    public void ReceiveAccel(float x, float y, float z)
    {
        Vector3 accel = new Vector3(x, y, z);
        Debug.Log("Received Accel: " + accel);
        float Roll = Mathf.Atan2(y, z) * 180 / Mathf.PI;
        float Pitch = Mathf.Atan2(-x, Mathf.Sqrt(y * y + z * z)) * 180 / Mathf.PI;
        transform.rotation = Quaternion.Euler(Pitch, 0, Roll);
    }
}