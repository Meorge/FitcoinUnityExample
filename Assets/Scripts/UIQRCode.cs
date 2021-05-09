using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIQRCode : MonoBehaviour
{
    public Image overlayImage = null, loadingIndicator = null;
    public RawImage qrImage = null;

    public Sequence loadingIndicatorAnimator = null;

    public float fadeDuration = 0.5f;

    public void Start() {
        loadingIndicatorAnimator = DOTween.Sequence()
        .Append(loadingIndicator.rectTransform.DOLocalRotate(new Vector3(0f, 0f, -360), 1f, RotateMode.LocalAxisAdd))
        .SetEase(Ease.Linear)
        .SetLoops(-1)
        .Pause();
    }

    public void StartLoading() {
        qrImage.texture = null;
        overlayImage.DOFade(1f, 0f);
        loadingIndicator.DOFade(1f, 0f);
        loadingIndicatorAnimator.Restart();
    }

    public void DoneLoading(Texture texture) {
        qrImage.texture = texture;
        DOTween.Sequence()
        .Append(loadingIndicator.DOFade(0f, fadeDuration))
        .Append(overlayImage.DOFade(0f, fadeDuration))
        .AppendCallback(() => { loadingIndicatorAnimator.Pause(); });
    }

    public void Reset() {
        overlayImage.DOFade(1f, 0f);
        loadingIndicator.DOFade(1f, 0f);
        loadingIndicatorAnimator.Pause();
    }
}
