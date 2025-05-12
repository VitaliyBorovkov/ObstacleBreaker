using DG.Tweening;

using UnityEngine;

public class PathZoneResizer : MonoBehaviour
{
    [SerializeField] private float resizeDuration = 0.3f;
    [SerializeField] private float widthMultiplier = 1f;

    private float lastTargetWidth;

    public void ResizeTo(float playerScaleX)
    {
        float targetWidth = playerScaleX * widthMultiplier;

        if (Mathf.Approximately(targetWidth, lastTargetWidth))
            return;

        lastTargetWidth = targetWidth;

        Vector3 newScale = transform.localScale;
        newScale.x = targetWidth;

        transform.DOScale(newScale, resizeDuration).SetEase(Ease.OutSine);
    }
}