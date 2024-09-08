using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurveyMenu : MonoBehaviour
{

    [SerializeField] private float scrollSensitivity;
    [SerializeField] private SurveyOption[] painOptions, discomfortOptions, ptOptions;
    private bool painChosen, discomfortChosen, ptChosen;
    private float choiceIndex;
    private float goIndex;

    private int comfortValue, painValue, helpfulValue;
    private int choiceLastFrame;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(choiceLastFrame != Mathf.RoundToInt(choiceIndex))
        {
            FXManager.Instance.PlaySFX("UIClick", 1f);
            choiceLastFrame = Mathf.RoundToInt(choiceIndex);
        }

        if (!HardwareManager.Instance.HardwareObjects.ContainsKey(0) || !HardwareManager.Instance.HardwareObjects.ContainsKey(1))
        {
            return;
        }

        if (!painChosen)
        {
            choiceIndex += scrollSensitivity * HardwareManager.Instance.HardwareObjects[0].GetDirectionValue(Axis.Y);
            if (choiceIndex < 0)
            {
                choiceIndex = 4;
            }
            if (choiceIndex > 4)
            {
                choiceIndex = 0;
            }

            for (int i = 0; i < painOptions.Length; i++)
            {
                painOptions[i].SetSelected(i == Mathf.RoundToInt(choiceIndex));
            }

            Debug.Log(HardwareManager.Instance.HardwareObjects[1].GetDirectionValue(Axis.Y));

            goIndex += scrollSensitivity * HardwareManager.Instance.HardwareObjects[1].GetDirectionValue(Axis.Y);
            if (goIndex > 2)
            {
                FXManager.Instance.PlaySFX("UISelect", 1f);
                // LOCK IN
                painChosen = true;
                painOptions[Mathf.RoundToInt(choiceIndex)].GetComponent<Image>().color = Color.blue;
                painValue = Mathf.RoundToInt(choiceIndex) + 1;
                choiceIndex = 0;
                goIndex = 0;
            }

            return;

        }

        if (!discomfortChosen)
        {
            choiceIndex += scrollSensitivity * HardwareManager.Instance.HardwareObjects[0].GetDirectionValue(Axis.Y);
            if (choiceIndex < 0)
            {
                choiceIndex = 4;
            }
            if (choiceIndex > 4)
            {
                choiceIndex = 0;
            }

            for (int i = 0; i < discomfortOptions.Length; i++)
            {
                discomfortOptions[i].SetSelected(i == Mathf.RoundToInt(choiceIndex));
            }

            goIndex += scrollSensitivity * HardwareManager.Instance.HardwareObjects[1].GetDirectionValue(Axis.Y);
            if (goIndex > 2)
            {
                // LOCK IN
                FXManager.Instance.PlaySFX("UISelect", 1f);
                discomfortChosen = true;
                discomfortOptions[Mathf.RoundToInt(choiceIndex)].GetComponent<Image>().color = Color.blue;
                comfortValue = Mathf.RoundToInt(choiceIndex) + 1;
                choiceIndex = 0;
                goIndex = 0;
            }

            return;

        }

        if (!ptChosen)
        {
            choiceIndex += scrollSensitivity * HardwareManager.Instance.HardwareObjects[0].GetDirectionValue(Axis.Y);
            if (choiceIndex < 0)
            {
                choiceIndex = 1;
            }
            if (choiceIndex > 1)
            {
                choiceIndex = 0;
            }

            for (int i = 0; i < ptOptions.Length; i++)
            {
                ptOptions[i].SetSelected(i == Mathf.RoundToInt(choiceIndex));
            }

            goIndex += scrollSensitivity * HardwareManager.Instance.HardwareObjects[1].GetDirectionValue(Axis.Y);
            if (goIndex > 2)
            {
                // LOCK IN
                FXManager.Instance.PlaySFX("UISelect", 1f);
                ptChosen = true;
                ptOptions[Mathf.RoundToInt(choiceIndex)].GetComponent<Image>().color = Color.blue;
                helpfulValue = Mathf.RoundToInt(choiceIndex) + 1;
                choiceIndex = 0;
                goIndex = 0;
            }

            return;

        }

        if(float.IsNaN(choiceIndex))
        {
            choiceIndex = 0;
        }

        if(float.IsNaN(goIndex))
        {
            goIndex = 0;
        }


    }

    public int GetPainValue()
    {
        return painValue;
    }

    public int GetComfortValue()
    {
        return comfortValue;
    }

    public int GetHelpfulValue()
    {
        return helpfulValue;
    }

}
