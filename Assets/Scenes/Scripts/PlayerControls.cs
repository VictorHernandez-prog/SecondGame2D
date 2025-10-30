using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerControls : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
     Rigidbody2D rb;
     Collider2D coll;
    SpriteRenderer sr;
    HideInLocker Hide;
    Vector3 movement;

    public enum PlayerState { Idle, Walking, Sprinting }
    public PlayerState s = PlayerState.Idle;
    public float forceEnds = 0.1f;
    public float Speed;
    public float Walking = 60f;
    public float Running = 90f;
    public float Horizontal;
    public float Vertical;
    void Start()
    {
        Hide = GetComponent<HideInLocker>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame

    void Update()
    {
        if (!Hide.isPlayerHiding) // disable movement while hiding
        {
            ManageInput();
            SwitchStates();
            HandleMovementState();
        }

    }
    void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(movement.x * Speed, movement.y * Speed, movement.z * Speed);
    }


    void ManageInput()
    {
        movement = new Vector3(Horizontal, Vertical, 0).normalized;
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            Speed = Running;
        }
        else
        {
            Speed = Walking;
        }
    }

    void SwitchStates()
    {
        switch (s)
        {
            case PlayerState.Idle:
                sr.color = Color.red;
                break;

            case PlayerState.Walking:
                sr.color = Color.blue;
                break;

            case PlayerState.Sprinting:
                sr.color = Color.green;
                break;
        }
    }
    public void EnterLocker()
    {
        Physics2D.IgnoreLayerCollision(6, 7, true);
        sr.sortingOrder = 0;
        rb.linearVelocity = Vector2.zero;
        rb.Sleep();
        Debug.Log("Player is hiding in locker.");
    }

    public void ExitLocker()
    {
        Physics2D.IgnoreLayerCollision(6, 7, false);
        sr.sortingOrder = 2;
        rb.WakeUp();
        Debug.Log("Player exited locker.");
    }

    void HandleMovementState()
    {
        Vector2 velocity = rb.linearVelocity;

        if (velocity.magnitude > forceEnds)
        {
            if (Input.GetKey(KeyCode.LeftShift))
                s = PlayerState.Sprinting;
            else
                s = PlayerState.Walking;
        }
        else
        {
            s = PlayerState.Idle;
        }
    }
}
