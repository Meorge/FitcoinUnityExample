using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class UIModalManager : MonoBehaviour
{
    [SerializeField]
    private RectTransform cardRectTransform = null;

    [SerializeField]
    private Image backdrop = null;

    public float animationDuration = 0.5f;

    public void DismissModal() {
        DOTween.Sequence()
        .Append(cardRectTransform.DOAnchorPosY(-cardRectTransform.sizeDelta.y * 1.5f, animationDuration))
        .Join(backdrop.DOFade(0f, animationDuration))
        .AppendCallback(() => { backdrop.raycastTarget = false; })
        ;
    }

    public void ShowModal() {
        DOTween.Sequence()
        .AppendCallback(() => { backdrop.raycastTarget = true; })
        .Join(cardRectTransform.DOAnchorPosY(0, animationDuration))
        .Join(backdrop.DOFade(0.75f, animationDuration))
        ;
    }
}
