using UnityEngine;

public class Lock : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Hide Spot for Player")]
    public Transform HidingSpot; // assign in Inspector or find child automatically

    private void Reset()
    {
        if (transform.Find("HidingSpot") != null)
            HidingSpot = transform.Find("HidingSpot");
    }
}
