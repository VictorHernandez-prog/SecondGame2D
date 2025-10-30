using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorWin : MonoBehaviour
{
    private bool isPlayerNear = false;

    [Header("Scene Settings")]
    public string WinSCene = "WinScene"; // ← Set this in the Inspector

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            WinGame();
        }
    }

    void WinGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            Debug.Log("Player near door");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            Debug.Log("Player left door");
        }
    }
}