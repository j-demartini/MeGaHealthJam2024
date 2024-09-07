using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetricManager : MonoBehaviour
{

    public static MetricManager Instance { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            NotificationManager.Instance.CreateNotification("New Device Connected", "Leg tracker ID:0 has been identified.", 2.5f);
        }
    }



}
