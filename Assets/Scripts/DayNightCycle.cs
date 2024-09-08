using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public static DayNightCycle Instance { get; private set; }

    private Rotate rotate;

    public bool IsDay
    {
        get
        {
            if (transform.eulerAngles.x > 180 && transform.eulerAngles.x < 360)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        rotate = GetComponent<Rotate>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.eulerAngles.x > 360)
        {
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
        }
    }
}
