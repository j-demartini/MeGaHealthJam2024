using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum AffectorType
{ 

    None,
    TypeIn
}

public class TextAffector : MonoBehaviour
{

    [SerializeField] private AffectorType type;
    private TMP_Text text;
    private float typeInTime = .8f;

    void OnEnable()
    {
        text = GetComponent<TMP_Text>();

        switch (type)
        {

            case AffectorType.TypeIn:
                TypeIn();
                break;
        }

    }

    public void TypeIn()
    {
        StartCoroutine(TypeInRoutine());
        text.CrossFadeAlpha(0f, 0f, true);
        text.CrossFadeAlpha(1f, 3f, true);
    }

    private IEnumerator TypeInRoutine()
    {
        string t = text.text;
        text.text = "";
        for (int i = 0; i < t.Length; i++)
        {
            text.text += t[i];
            yield return new WaitForSeconds(typeInTime / (float)t.Length);
        }
        yield return new WaitForSeconds(1f);
    }


}
