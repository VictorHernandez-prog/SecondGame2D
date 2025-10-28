using UnityEngine;

public class HideInLocker : MonoBehaviour
{
    private bool isPlayerClose = false;
    private PlayerControls playerControls;
    public bool isPlayerHiding { get; private set; } = false;
    private Lock currentLocker;
    private Vector3 lastPlayerPosition; // <-- NEW: store where you entered

    private void Start()
    {
        playerControls = GetComponent<PlayerControls>();
    }

    private void Update()
    {
        if (isPlayerClose && Input.GetKeyDown(KeyCode.E))
        {
            if (currentLocker == null) return;

            isPlayerHiding = !isPlayerHiding;

            if (isPlayerHiding)
            {
                // Save where player was before hiding
                lastPlayerPosition = transform.position;

                // Move into locker
                transform.position = currentLocker.HidingSpot.position;
                playerControls?.EnterLocker();

                Debug.Log("Player entered locker. Saved position: " + lastPlayerPosition);
            }
            else
            {
                // Move back to where player entered
                transform.position = lastPlayerPosition;

                playerControls?.ExitLocker();

                Debug.Log("Player exited locker to saved position: " + lastPlayerPosition);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Lockers"))
        {
            isPlayerClose = true;
            currentLocker = other.GetComponentInParent<Lock>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Lockers"))
        {
            isPlayerClose = false;
            currentLocker = null;
        }
    }
}