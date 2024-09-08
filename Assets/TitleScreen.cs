using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{

    public GameObject co;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown)
        {
            StartCoroutine(End());
        }
    }

    public IEnumerator End()
    {
        foreach(MaskableGraphic graphic in GetComponentsInChildren<MaskableGraphic>())
        {
            graphic.CrossFadeAlpha(0f, 1f, false);
        }
        yield return new WaitForSeconds(0.5f);
        co.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }

}
