using UnityEngine;

public class HideInLocker : MonoBehaviour
{
    private bool isPlayerClose = false;
    private PlayerControls Player;
    public bool isPlayerHiding { get; private set; } = false;
    private Lock currentLocker;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = GetComponent<PlayerControls>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerClose && Input.GetKeyDown(KeyCode.E))
        {
            isPlayerHiding = !isPlayerHiding;

            if (isPlayerHiding )
            {
                transform.position = currentLocker.HidingSpot.position;
                Player.EnterLocker();
            }
            else
            {
                Player.ExitLocker();
            }
        }
    }   
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Lockers"))
            isPlayerClose = true;
        currentLocker = other.GetComponent<Lock>();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Lockers"))
            isPlayerClose = false;
        currentLocker = null;
    }

}
