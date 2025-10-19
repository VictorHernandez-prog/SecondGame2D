using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
     Rigidbody2D rb;
     Collider2D coll;
    SpriteRenderer sr;
    HideInLocker Hide;

    public enum PlayerState { Idle, Vertical, Horizontal }
    public PlayerState s = PlayerState.Idle;
    public float forceEnds = 0.1f;
    public float Speed = 0.5f;
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
            HandleMovementState();
        }
        SwitchStates();

    }

    void ManageInput()
    {
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(Vector3.right * Speed);
        }

        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(Vector3.left * Speed);
        }

        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(Vector3.up * Speed);
        }

        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(Vector3.down * Speed);
        }
    }

    void SwitchStates()
    {
        switch(s) 
        {
            case PlayerState.Idle:
                sr.color = Color.red;
                    break;

            case PlayerState.Vertical:
                sr.color = Color.blue;
                    break;

            case PlayerState.Horizontal:
                sr.color = Color.black;
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
            if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y))
            {
                s = PlayerState.Horizontal;
            }
            else
            {
            s = PlayerState.Vertical;
            }
        }
        else
        {
            s = PlayerState.Idle;
        }
    }
}
