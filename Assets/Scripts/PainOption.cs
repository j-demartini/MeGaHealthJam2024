using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurveyOption : MonoBehaviour
{

    private Image border;

    void Awake()
    {
        border = GetComponent<Image>();
    }

    public void SetSelected(bool b)
    {
        border.color = b ? Color.yellow : Color.white;
    }

}