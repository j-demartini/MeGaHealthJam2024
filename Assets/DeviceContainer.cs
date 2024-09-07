using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeviceContainer : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    private Hardware hardware;

    public void SetHardware(Hardware hardware)
    {
        this.hardware = hardware;
        text.SetText("Tracker " + hardware.ID + " | Status: <color=green>Receiving");
    }

}
