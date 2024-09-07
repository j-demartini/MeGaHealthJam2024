using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text messageText;

    public void SetData(string title, string message, float duration)
    {
        titleText.SetText(title);
        messageText.SetText(message);

        Fade(false, 0f);
        Fade(true, 0.25f);
        StartCoroutine(DelayFadeOut(duration));

    }

    public void Fade(bool visible, float duration)
    {
        foreach(MaskableGraphic graphic in GetComponentsInChildren<MaskableGraphic>())
        {
            graphic.CrossFadeAlpha(visible ? 1f : 0f, duration, true);
        }
    }

    public IEnumerator DelayFadeOut(float f)
    {
        yield return new WaitForSeconds(f);
        Fade(false, 0.5f);
        Destroy(gameObject, 0.5f);
    }

}
