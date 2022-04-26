using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using System;

public static class UIAnimationHelper 
{
    public static IEnumerator WaitAndFadeOut(TextMeshProUGUI message, Image background, float time)
    {
        yield return new WaitForSeconds(time);
        Color fadeoutColor = background.color;
        fadeoutColor.a = 0;
        background.DOColor(fadeoutColor, 0.9f).Play();
        fadeoutColor = message.color;
        fadeoutColor.a = 0;
        message.DOColor(fadeoutColor, 0.9f).Play();
    }

    public static IEnumerator WaitAndFadeOut(TextMeshProUGUI message, float time)
    {
        yield return new WaitForSeconds(time);      
        Color fadeoutColor = message.color;
        fadeoutColor.a = 0;
        message.DOColor(fadeoutColor, 0.25f).Play();
    }

    public static IEnumerator WaitAndScaleDown(Transform target, float time)
    {
        target.DOScale(Vector3.one * 1.5f, 0.1f).Play();
        yield return new WaitForSeconds(time);      
        target.DOScale(Vector3.one * 0.2f, 0.9f).Play();
    }

    public static IEnumerator DriftUp(Transform target, float time)
    {
        target.DOMoveY(target.position.y + 75f, time, false).Play();
        yield return null;
    }
}
