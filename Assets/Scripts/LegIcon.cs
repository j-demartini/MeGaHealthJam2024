using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LegIcon : MonoBehaviour
{
    [SerializeField] private int trackerID;
    [SerializeField] private Transform leg;

    void Update()
    {
        if (HardwareManager.Instance.HardwareObjects.ContainsKey(trackerID))
        {
            Hardware h = HardwareManager.Instance.HardwareObjects[trackerID];
            leg.localRotation = Quaternion.Euler(0, 0, h.Sum.y);
        }
    }
}
