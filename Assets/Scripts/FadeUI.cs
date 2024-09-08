using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeUI : MonoBehaviour
{
    [SerializeField] private bool startVisible = false;
    [Space]
    [SerializeField] private float fadeTime = 0.5f;

    private List<MaskableGraphic> graphics;

    protected virtual void Awake()
    {
        graphics = new List<MaskableGraphic>();
        ReInitialize();
    }

    public void ReInitialize()
    {
        graphics.Clear();
        foreach (MaskableGraphic graphic in GetComponentsInChildren<MaskableGraphic>(false))
        {
            graphics.Add(graphic);

            if (!startVisible)
            {
                graphic.CrossFadeAlpha(0f, 0f, true);
            }
        }
    }

    public void Display(bool display)
    {
        if (display)
            StartCoroutine(Activate());
        else
            StartCoroutine(Deactivate());
    }

    private IEnumerator Fade(bool fadeIn)
    {
        foreach (MaskableGraphic graphic in graphics)
        {
            graphic.CrossFadeAlpha(fadeIn ? 1f : 0f, fadeTime, false);
        }

        yield return new WaitForSeconds(fadeTime);
    }

    private IEnumerator Activate()
    {
        yield return Fade(true);
    }

    private IEnumerator Deactivate()
    {
        yield return Fade(false);
    }
}
