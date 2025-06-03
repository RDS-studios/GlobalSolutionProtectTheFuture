using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpecialMovement : MonoBehaviour
{

    [Header("References")]
    [SerializeField] PlayerInput playerInputs;
    [SerializeField] Animator animator;
    [SerializeField] Animator animator2;
    [SerializeField] PlayerStats playerStats;

    [Header("Movement Settings")]
    [SerializeField] float moveSpeed = 5f;

    private Vector2 moveInput;
    private int facingDirection = 1;

    void Start()
    {
        playerInputs = GetComponent<PlayerInput>();
        playerStats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (!playerStats.canMove) return;

        moveInput = playerInputs.actions["Move"].ReadValue<Vector2>();

        // Move the player using transform
        Vector3 moveDelta = new Vector3(moveInput.x, moveInput.y, 0) * moveSpeed * Time.deltaTime;
        transform.position += moveDelta;

        // Handle animations
        bool isMoving = moveInput.magnitude > 0.1f;
        animator.SetBool("isWalk", isMoving);
        animator2.SetBool("isWalk", isMoving);

        // Handle left/right flipping
        //if (moveInput.x > 0)
        //    facingDirection = 1;
        //else if (moveInput.x < 0)
        //    facingDirection = -1;

        Vector3 scale = transform.localScale;
        scale.x = facingDirection;
        transform.localScale = scale;
    }
}
