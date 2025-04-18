using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryEffectManager : MonoBehaviour
{
    public CanvasGroup parryPopup;
    public float popupDuration = 0.4f;

    public void ShowParryPopup()
    {
        StopAllCoroutines();
        StartCoroutine(FadePopup());
    }

    IEnumerator FadePopup()
    {
        parryPopup.alpha = 1f;
        yield return new WaitForSecondsRealtime(popupDuration);
        parryPopup.alpha = 0f;
    }
}
