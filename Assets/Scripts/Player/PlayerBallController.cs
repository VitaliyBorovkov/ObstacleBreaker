using System;

using UnityEngine;

public class PlayerBallController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private PathZoneResizer pathZoneResizer;

    [SerializeField] private float minPlayerScale = 0.3f;
    [SerializeField] private float scaleLossPerSecond = 0.5f;
    [SerializeField] private float projectileGrowthRate = 1f;
    [SerializeField] private float projectileSpeed = 10f;

    private GameObject currentProjectile;
    private float chargeTimer;
    private bool isCharging;
    private Vector3 initialScale;

    public Action OnPlayerTooSmall;
    public bool IsCharging => isCharging;

    private void Start()
    {
        initialScale = transform.localScale;
    }

    private void Update()
    {
        if (IsTouchStarted())
        {
            if (!isCharging)
                StartCharging();
        }

        if (isCharging)
        {
            ChargeProjectile();
        }

        if (IsTouchEnded())
        {
            RealiseProjectile();
        }
    }

    private bool IsTouchStarted()
    {
#if UNITY_EDITOR
        return Input.GetMouseButtonDown(0);
#else
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
#endif
    }

    private bool IsTouchEnded()
    {
#if UNITY_EDITOR
        return Input.GetMouseButtonUp(0);
#else
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended;
#endif
    }

    private void StartCharging()
    {
        if (transform.localScale.x <= minPlayerScale)
        {
            OnPlayerTooSmall?.Invoke();

            gameManager?.LoseGame();
            return;
        }

        isCharging = true;

        currentProjectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        currentProjectile.transform.localScale = Vector3.zero;

        ProjectileController controller = currentProjectile.GetComponent<ProjectileController>();
        if (controller != null)
        {
            controller.OnExploded += HandleProjectileExploded;
        }

        chargeTimer = 0f;
    }

    private void ChargeProjectile()
    {
        chargeTimer += Time.deltaTime;

        float growth = projectileGrowthRate * Time.deltaTime;

        if (currentProjectile != null)
        {
            currentProjectile.transform.localScale += Vector3.one * growth;
        }

        transform.localScale -= Vector3.one * growth * scaleLossPerSecond;
        pathZoneResizer.ResizeTo(transform.localScale.x);

        if (transform.localScale.x <= minPlayerScale)
        {
            Debug.Log("PlayerBallController: Player size reached minimum during charge.");
            RealiseProjectile();

            gameManager?.LoseGame();
        }
    }

    private void RealiseProjectile()
    {
        if (!isCharging)
            return;

        isCharging = false;

        if (currentProjectile != null)
        {
            if (currentProjectile.transform.localScale.magnitude < 0.01f)
            {
                currentProjectile.transform.localScale = Vector3.one * 0.1f;
            }

            Rigidbody rb = currentProjectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.forward * projectileSpeed;
            }
        }

        chargeTimer = 0f;
    }

    private void HandleProjectileExploded()
    {
        currentProjectile = null;
        isCharging = false;
    }
}