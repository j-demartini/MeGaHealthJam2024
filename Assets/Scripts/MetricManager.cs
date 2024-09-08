using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MetricManager : MonoBehaviour
{

    public static MetricManager Instance { get; private set; }
    [SerializeField] private LineRenderer r;

    private Hardware leftWheel;
    private Hardware rightWheel;
    private Hardware leg;

    // Metrics variables

    // Leg extensions
    private bool legRaised;
    private int legExtensions;

    private float maxExtensionHeight;
    private float highestEverExtension;
    private float legExtensionHeightSum;

    // Distance traveled
    private float distanceTraveled;
    private float gameTimer;

    // Points
    private Dictionary<float, float> lineValues = new Dictionary<float, float>();
    private List<Vector3> linePositions = new List<Vector3>();
    private bool populatingLine;
    private int currentLineIndex;
    private float lineTimer;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if(!leftWheel && HardwareManager.Instance.HardwareObjects.ContainsKey(0))
        {
            leftWheel = HardwareManager.Instance.HardwareObjects[0];
        }
        if (!rightWheel && HardwareManager.Instance.HardwareObjects.ContainsKey(1))
        {
            rightWheel = HardwareManager.Instance.HardwareObjects[1];
        }
        if (!leg && HardwareManager.Instance.HardwareObjects.ContainsKey(0))
        {
            leg = HardwareManager.Instance.HardwareObjects[0];
        }


        if(leftWheel && rightWheel && leg)
        {
            // Collect Metrics
            LegExtensions();
            DistanceTraveled();
        }

        if(leg && GameManager.Instance.GameStarted)
        {
            if (lineTimer > 0.2f)
            {
                lineValues.Add(gameTimer, leg.GetSumValue());
                lineTimer = 0;
            }
            lineTimer += Time.deltaTime;
        }

        if(GameManager.Instance.GameStarted)
        gameTimer += Time.deltaTime;

        if(populatingLine)
        {
            r.positionCount = currentLineIndex;
            r.SetPositions(linePositions.GetRange(0, currentLineIndex).ToArray());
            currentLineIndex++;
            if(currentLineIndex > lineValues.Values.Count)
            {
                populatingLine = false;
            }
        }

    }

    public void LegExtensions()
    {
        if(leg.GetSumValue() > 0.8f && !legRaised)
        {
            legRaised = true;
        }

        if(legRaised && leg.GetSumValue() > 0.8f)
        {
            if(leg.Sum.y > maxExtensionHeight)
            {
                maxExtensionHeight = leg.Sum.y;
                if(maxExtensionHeight > highestEverExtension)
                {
                    highestEverExtension = maxExtensionHeight;
                }
            }
        }

        if(leg.GetSumValue() <= 0.8f && legRaised)
        {
            legRaised = false;
            legExtensions++;
            legExtensionHeightSum += maxExtensionHeight;
            maxExtensionHeight = 0;
        }
    }

    public void DistanceTraveled()
    {
        float velLeft = leftWheel.GetDirectionValue(Axis.Y);
        float velRight = rightWheel.GetDirectionValue(Axis.Y);
        if((velLeft > 0 && velRight > 0) || (velLeft < 0 && velRight < 0))
        {                                               // radius in meters of the chair
            distanceTraveled += Mathf.Abs((velLeft * (1f / 119f) * Mathf.Deg2Rad) * 0.75f);
        }
    }

    public void PopulateLine()
    {
        populatingLine = true;
        r.transform.localScale = new Vector3(1f, 1f, 1f);
        foreach (float f in lineValues.Keys)
        {
            linePositions.Add(new Vector3(f * (4f / gameTimer), lineValues[f] / 4f, 0));
        }
    }

    public float GetDistanceTravelled()
    {
        return distanceTraveled;
    }

    public float GetAvgLegAngle()
    {
        return legExtensionHeightSum / legExtensions;
    }

    public float GetMaxLegAngle()
    {
        return maxExtensionHeight;
    }

    public int GetRaises()
    {
        return legExtensions;
    }

}
