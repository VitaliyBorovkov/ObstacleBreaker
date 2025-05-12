using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private GameManager gameManager;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered)
        {
            return;
        }

        if (other.CompareTag(playerTag))
        {
            triggered = true;
            Debug.Log("FinishTrigger: Player entered finish zone.");
            gameManager.WinGame();
        }
    }
}
