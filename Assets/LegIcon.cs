using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LegIcon : MonoBehaviour
{
    [SerializeField] private int trackerID;
    [SerializeField] private Transform leg;
    [SerializeField] private TMP_Text minValue, maxValue;
    [SerializeField] private RectTransform meter;

    void Update()
    {
        if (HardwareManager.Instance.HardwareObjects.ContainsKey(trackerID))
        {
            Hardware h = HardwareManager.Instance.HardwareObjects[trackerID];
            icon.Rotate(new Vector3(0, 0, h.Direction.y));
            minValue.SetText(h.MinValues.y.ToString("0.0"));
            maxValue.SetText(h.MaxValues.y.ToString("0.0"));
            meter.anchoredPosition = new Vector2(h.Direction.y / h.MaxValues.y * 165f, 0);
        }
    }
}
