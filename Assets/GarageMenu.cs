using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarageMenu : MonoBehaviour
{

    public static GarageMenu Instance { get; private set; }

    [Header("Choices")]
    [SerializeField] private List<ColorOption> colorOptions = new List<ColorOption>();
    [SerializeField] private List<HatOption> hatOptions = new List<HatOption>();


    [SerializeField] private MeshRenderer plane;
    [SerializeField] private List<GameObject> hats = new List<GameObject>();
    [SerializeField] private float scrollSensitivity;

    private bool colorChosen;
    private float choiceIndex;
    private float goIndex;

    void Awake()
    {
        Instance = this;    
    }

    void Update()
    {
        if(!colorChosen)
        {
            choiceIndex += scrollSensitivity * HardwareManager.Instance.HardwareObjects[0].GetDirectionValue(Axis.Y);
            if(choiceIndex < 0)
            {
                choiceIndex = 5;
            }
            if(choiceIndex > 5)
            {
                choiceIndex = 0;
            }

            for(int i = 0; i < colorOptions.Count; i++)
            {
                colorOptions[i].SetSelected(i == Mathf.RoundToInt(choiceIndex));
            }

            goIndex += scrollSensitivity * HardwareManager.Instance.HardwareObjects[0].GetDirectionValue(Axis.Y);
            if(goIndex > 2)
            {

            }

        }



    }

    public MeshRenderer GetPlane()
    {
        return plane;
    }

}
