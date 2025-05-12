using UnityEngine;

public class FinishGate : MonoBehaviour
{
    [SerializeField] private Transform gateTransform;
    [SerializeField] private float openHeight = 3f;
    [SerializeField] private float openSpeed = 1f;
    [SerializeField] private float triggerDistance = 5f;
    [SerializeField] private string playerTag = "Player";

    private bool isOpening = false;
    private bool isOpen = false;
    private Vector3 initialGatePosition;
    private Vector3 targetGatePosition;
    private Transform playerTransform;

    public bool IsGateOpen => isOpen;

    private void Start()
    {
        initialGatePosition = gateTransform.position;
        targetGatePosition = initialGatePosition + Vector3.up * openHeight;

        GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        else
        {
            Debug.LogError("FinishGate: Player with tag 'Player' not found.");
        }
    }

    private void Update()
    {
        if (!isOpen && playerTransform != null)
        {
            float distance = Vector3.Distance(playerTransform.position, gateTransform.position);
            if (distance <= triggerDistance)
            {
                isOpening = true;
                Debug.Log("FinishGate: Player approached, opening gate...");
            }
        }

        if (isOpening && !isOpen)
        {
            gateTransform.position = Vector3.MoveTowards(
                gateTransform.position, targetGatePosition, openSpeed * Time.deltaTime);

            if (Vector3.Distance(gateTransform.position, targetGatePosition) < 0.1f)
            {
                isOpen = true;
                Debug.Log("FinishGate: Gate opened.");
            }
        }
    }
}