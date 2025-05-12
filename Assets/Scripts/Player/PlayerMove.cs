using DG.Tweening;

using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private PlayerBallController playerBallController;
    [SerializeField] private FinishGate finishGate;
    [SerializeField] private Transform gateTransform;
    [SerializeField] private LayerMask obstacleLayer;

    [SerializeField] private float checkWidthMultiplier = 1f;
    [SerializeField] private float jumpDistance = 1f;
    [SerializeField] private float jumpDuration = 0.3f;
    [SerializeField] private float jumpHeight = 1f;
    [SerializeField] private int jumpCount = 1;
    [SerializeField] private float stopDistanceToGate = 5f;

    private Transform playerTransform;
    private bool isJumping = false;
    private float squashScaleY = 0.7f;
    private float squashScaleXZ = 1.1f;
    private float squashDuration = 0.1f;
    private float unsquashDuration = 0.05f;

    private void Awake()
    {
        playerTransform = transform;
    }

    private void Update()
    {
        if (playerBallController != null && playerBallController.IsCharging)
            return;

        if (isJumping)
            return;

        if (ShouldWaitForGate())
        {
            Debug.Log("PlayerMove: Waiting for gate to open.");
            return;
        }

        TryJump();
    }

    private bool ShouldWaitForGate()
    {
        if (finishGate == null || finishGate.IsGateOpen)
            return false;

        float distance = Vector3.Distance(playerTransform.position, gateTransform.position);
        return distance <= stopDistanceToGate;
    }

    private void TryJump()
    {
        float radius = playerTransform.localScale.x * checkWidthMultiplier;
        Vector3 targetPosition = playerTransform.position + Vector3.forward * jumpDistance;
        targetPosition.y = playerTransform.position.y;

        bool blocked = Physics.CheckSphere(targetPosition, radius, obstacleLayer);

        if (!blocked)
        {
            JumpTo(targetPosition);
        }
    }

    private void JumpTo(Vector3 targetPosition)
    {
        isJumping = true;
        Sequence jumpSequence = DOTween.Sequence();

        Vector3 originalScale = playerTransform.localScale;
        Vector3 compressedScale = new Vector3(
            originalScale.x * squashScaleXZ,
            originalScale.y * squashScaleY,
            originalScale.z * squashScaleXZ);

        jumpSequence.Append(playerTransform.DOScale(compressedScale, squashDuration))
                    .Append(playerTransform.DOScale(originalScale, unsquashDuration))
                    .Append(playerTransform.DOJump(targetPosition, jumpHeight, jumpCount, jumpDuration))
                    .OnComplete(() => isJumping = false);
    }
}