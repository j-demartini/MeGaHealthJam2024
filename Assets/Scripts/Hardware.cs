using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hardware : MonoBehaviour
{
    public void ReceiveGyro(float x, float y, float z)
    {
        Vector3 gyro = new Vector3(x, y, z);
        Debug.Log("Received Gyro: " + gyro);
        // TODO: Implement this method
    }

    public void ReceiveAccel(float x, float y, float z)
    {
        Vector3 accel = new Vector3(x, y, z);
        Debug.Log("Received Accel: " + accel);
        // TODO: Implement this method
    }
}