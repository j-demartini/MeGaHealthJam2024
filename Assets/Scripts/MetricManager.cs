using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetricManager : MonoBehaviour
{

    public static MetricManager Instance { get; private set; }

    private Hardware leftWheel;
    private Hardware rightWheel;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
                   
    }

}
