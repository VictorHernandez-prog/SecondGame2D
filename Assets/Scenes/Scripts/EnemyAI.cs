using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum AIState { Roam, Chase, Hide }
    public AIState currentState = AIState.Roam;

    [Header("References")]
    public Transform player;
    private HideInLocker playerHide;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    [Header("AI Settings")]
    public Transform[] waypoints;
    public float roamSpeed = 4f;
    public float chaseSpeed = 7f;
    public float detectionRange = 20f;
    public float loseSightRange = 20f;
    public float idleDuration = 2f;

    private int currentWaypoint = 0;
    private Vector2 targetPosition;
    private float idleTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        playerHide = player.GetComponent<HideInLocker>();

        if (waypoints.Length > 0)
            targetPosition = waypoints[0].position;
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        bool isPlayerHiding = playerHide != null && playerHide.isPlayerHiding;

        HandleStates(distanceToPlayer, isPlayerHiding);
    }

    void HandleStates(float distanceToPlayer, bool isPlayerHiding)
    {
        switch (currentState)
        {
            case AIState.Roam:
                sr.color = Color.green;
                HandleRoam(distanceToPlayer, isPlayerHiding);
                break;

            case AIState.Chase:
                sr.color = Color.red;
                HandleChase(distanceToPlayer, isPlayerHiding);
                break;

            case AIState.Hide:
                sr.color = Color.blue;
                HandleHide();
                break;
        }
    }

    void HandleRoam(float distanceToPlayer, bool isPlayerHiding)
    {
        if (!isPlayerHiding && distanceToPlayer < detectionRange)
        {
            currentState = AIState.Chase;
            return;
        }

        MoveTowards(targetPosition, roamSpeed);

        if (Vector2.Distance(transform.position, targetPosition) < 0.2f)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= idleDuration)
            {
                currentWaypoint = Random.Range(0, waypoints.Length);
                targetPosition = waypoints[currentWaypoint].position;
                idleTimer = 0f;
            }
        }
    }

    void HandleChase(float distanceToPlayer, bool isPlayerHiding)
    {
        if (isPlayerHiding)
        {
            currentState = AIState.Hide;
            return;
        }

        MoveTowards(player.position, chaseSpeed);

        if (distanceToPlayer > loseSightRange)
            currentState = AIState.Hide;
    }

    void HandleHide()
    {
        idleTimer += Time.deltaTime;
        if (idleTimer >= idleDuration * 2)
        {
            currentState = AIState.Roam;
            idleTimer = 0f;
        }
    }

    void MoveTowards(Vector2 target, float speed)
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        rb.linearVelocity = direction * speed;
    }
}