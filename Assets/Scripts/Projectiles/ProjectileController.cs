using System;

using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private float explosionDelay = 0.1f;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float explosionForce = 100f;
    [SerializeField] private float explosionUpwardModifier = 0.5f;
    [SerializeField] private float explosionRadiusMultiplier = 1.5f;
    [SerializeField] private float minExplosionRadius = 0.5f;

    private bool hasExploded;

    public Action OnExploded;

    private void OnCollisionEnter(Collision collision)
    {
        if (hasExploded)
            return;

        hasExploded = true;
        Invoke(nameof(Explode), explosionDelay);
    }

    private void Explode()
    {
        float radius = Mathf.Max(transform.localScale.x * explosionRadiusMultiplier, minExplosionRadius);
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, obstacleLayer);

        foreach (var hit in hitColliders)
        {
            Obstacle obstacle = hit.GetComponent<Obstacle>();
            if (obstacle != null)
            {
                obstacle.Infect();
            }

            Rigidbody rb = hit.attachedRigidbody;
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, radius, explosionUpwardModifier, ForceMode.Impulse);
            }
        }

        Debug.Log($"ProjectileController: Exploded with radius {radius}, hit {hitColliders.Length} obstacles.");

        OnExploded?.Invoke();
        Destroy(gameObject);
    }
}