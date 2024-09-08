using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorOption : MonoBehaviour
{

    [SerializeField] private Color color;
    [SerializeField] private Image colorImg;

    private Image border;

    void Awake()
    {
        border = GetComponent<Image>();    
    }

    // Start is called before the first frame update
    void Start()
    {
        colorImg.color = color;
    }

    public void SetSelected(bool b)
    {
        border.color = b ? Color.yellow : Color.white;
        if(b)
        {
            GarageMenu.Instance.GetPlane().material.color = color;
        }
    }

}
