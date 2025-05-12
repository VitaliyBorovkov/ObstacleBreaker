using DG.Tweening;

using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private GameObject destroyEffect;
    [SerializeField] private float destroyDelay = 0.5f;
    [SerializeField] private float pulseScale = 1.2f;
    [SerializeField] private float pulseDuration = 0.5f;
    [SerializeField] private Color infectedColor = Color.magenta;
    [SerializeField] private float colorChangeDuration = 0.2f;
    [SerializeField] private int pulseLoops = -1;


    private bool isInfected = false;
    private Material materialInstance;
    Vector3 originalScale;

    private void Awake()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            materialInstance = renderer.material;
        }

        originalScale = transform.localScale;
    }

    public void Infect()
    {
        if (isInfected)
        {
            return;
        }

        isInfected = true;

        Debug.Log($"Obstacle: {gameObject.name} infected.");

        AnimateInfection();
        Invoke(nameof(DestroyObstacle), pulseDuration);
    }

    private void AnimateInfection()
    {
        if (materialInstance != null)
        {
            materialInstance.DOColor(infectedColor, "_Color", colorChangeDuration);
        }

        transform.DOScale(originalScale * pulseScale, pulseDuration).SetLoops(pulseLoops, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    private void DestroyObstacle()
    {
        if (destroyEffect != null)
        {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
        }

        transform.DOKill();

        Destroy(gameObject, destroyDelay);
    }
}
