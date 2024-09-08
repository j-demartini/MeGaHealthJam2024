using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GarageMenu : MonoBehaviour
{

    public static GarageMenu Instance { get; private set; }

    [Header("Choices")]
    [SerializeField] private List<ColorOption> colorOptions = new List<ColorOption>();
    [SerializeField] private List<HatOption> hatOptions = new List<HatOption>();


    [SerializeField] private MeshRenderer plane;
    [SerializeField] private List<GameObject> hats = new List<GameObject>();
    [SerializeField] private float scrollSensitivity;

    private bool colorChosen, hatChosen;
    private float choiceIndex;
    private float goIndex;

    private int choiceLastFrame;

    void Awake()
    {
        Instance = this;    
    }

    void Update()
    {

        if (!HardwareManager.Instance.HardwareObjects.ContainsKey(0) || !HardwareManager.Instance.HardwareObjects.ContainsKey(1))
        {
            return;
        }

        if(choiceLastFrame != Mathf.RoundToInt(choiceIndex))
        {
            FXManager.Instance.PlaySFX("UIClick", 1f);
            choiceLastFrame = Mathf.RoundToInt(choiceIndex);
        }
        

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

            goIndex += scrollSensitivity * HardwareManager.Instance.HardwareObjects[1].GetDirectionValue(Axis.Y);
            if(goIndex > 2)
            {
                // LOCK IN
                FXManager.Instance.PlaySFX("UISelect", 1f);
                colorChosen = true;
                colorOptions[Mathf.RoundToInt(choiceIndex)].GetComponent<Image>().color = Color.blue;
                choiceIndex = 0;
                goIndex = 0;
            }

            return;

        }

        if (!hatChosen)
        {
            choiceIndex += scrollSensitivity * HardwareManager.Instance.HardwareObjects[0].GetDirectionValue(Axis.Y);
            if (choiceIndex < 0)
            {
                choiceIndex = 5;
            }
            if (choiceIndex > 5)
            {
                choiceIndex = 0;
            }

            for (int i = 0; i < hatOptions.Count; i++)
            {
                hatOptions[i].SetSelected(i == Mathf.RoundToInt(choiceIndex));
            }
            
            for(int i = 0; i < hats.Count; i++)
            {
                hats[i].SetActive(i == Mathf.RoundToInt(choiceIndex));
            }

            goIndex += scrollSensitivity * HardwareManager.Instance.HardwareObjects[1].GetDirectionValue(Axis.Y);
            if (goIndex > 2)
            {
                // LOCK IN
                Player.Instance.SetHat(Mathf.RoundToInt(choiceIndex));
                GameManager.Instance.Work = true;
                hatChosen = true;
                FXManager.Instance.PlaySFX("UISelect", 1f);
                hatOptions[Mathf.RoundToInt(choiceIndex)].GetComponent<Image>().color = Color.blue;
            }

        }



    }

    public MeshRenderer GetPlane()
    {
        return plane;
    }

}
