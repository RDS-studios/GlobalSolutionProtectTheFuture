using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovimentation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] PlayerInput playerInputs;
    [SerializeField] Rigidbody2D rb2d;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] Animator animator; 




    [Header("Movement Settings")]
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] bool canJump = true;

    [Header("Flip Settings")]
    [SerializeField] float flipSpeed = 10f; // Controls smoothness of flip
    private int facingDirection = 1; // 1 = right, -1 = left

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        playerInputs = GetComponent<PlayerInput>();
        playerStats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (playerStats.canMove)
        {
            Vector2 moveInput = playerInputs.actions["Move"].ReadValue<Vector2>();
            float moveX = moveInput.x;

            // Apply movement
            rb2d.linearVelocity = new Vector2(moveX * speed, rb2d.linearVelocity.y);
            // animation

            animator.SetBool("isWalk", moveX != 0);

            // Update facing direction if needed
            if (moveX > 0 && facingDirection != 1)
            {
                facingDirection = 1;
            }
            else if (moveX < 0 && facingDirection != -1)
            {
                facingDirection = -1;
            }
        }

        // Smoothly flip the character
        Vector3 currentScale = transform.localScale;
        float targetX = Mathf.Lerp(currentScale.x, facingDirection, Time.deltaTime * flipSpeed);
        transform.localScale = new Vector3(targetX, currentScale.y, currentScale.z);

        // Jump
        if (canJump && playerInputs.actions["Jump"].triggered)
        {
            rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            canJump = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            canJump = true;
        }
    }
}
