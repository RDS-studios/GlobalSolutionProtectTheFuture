using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovimentation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] PlayerInput playerInputs;
    [SerializeField] Rigidbody2D rb2d;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] Animator animator;
    [SerializeField] Animator animator2;

    [Header("Movement Settings")]
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] bool canJump = true;
    [SerializeField] float pullStrength = 10f; // adjust as needed
    [SerializeField] bool canPull = true; // whether the player can pull objects
    [SerializeField] float hookCd = 3.0f;

    [Header("Flip Settings")]
    [SerializeField] float flipSpeed = 10f;
    private int facingDirection = 1;

    [Header("Target Search")]
    public float searchRadius = 10f;
    public string targetTag = "Target";
    private GameObject previousTarget;

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

            // Animation
            animator.SetBool("isWalk", moveX != 0);
            animator2.SetBool("isWalk", moveX != 0);

            // Update facing direction
            if (moveX > 0 && facingDirection != 1)
                facingDirection = 1;
            else if (moveX < 0 && facingDirection != -1)
                facingDirection = -1;
        }

        // Smooth flip
        Vector3 currentScale = transform.localScale;
        float targetX = Mathf.Lerp(currentScale.x, facingDirection, Time.deltaTime * flipSpeed);
        transform.localScale = new Vector3(targetX, currentScale.y, currentScale.z);

        // Jump
        if (canJump && playerInputs.actions["Jump"].triggered)
        {
            rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            canJump = false;
            animator.SetBool("isJump", true);
            animator2.SetBool("isJump", true);
        }




        if (playerInputs.actions["Hook"].triggered && canPull)
        {
            GameObject currentTarget = FindClosestHighest();

            if (currentTarget != null)
            {
                Vector2 direction = (currentTarget.transform.position - transform.position).normalized;
                HookPoint hookShow = currentTarget.GetComponent<HookPoint>();
                if (hookShow != null)
                    hookShow.isAimed = false;

                rb2d.linearVelocity = direction * pullStrength;

                // Optional: Disable movement while being pulled
                playerStats.canMove = false;

                // Optional: Start coroutine to re-enable movement after some time
                StartCoroutine(EnableMovementAfterDelay(0.6f)); // 0.5 seconds delay
                StartCoroutine(HookCD()); // Start cooldown for pulling     
            }
        }








        if (true)
        {
            GameObject currentTarget = FindClosestHighest();

            if (currentTarget != previousTarget)
            {
                // Reset previous target
                if (previousTarget != null)
                {
                   HookPoint hookShow = previousTarget.GetComponent<HookPoint>();
                    if (hookShow != null)
                        hookShow.isAimed = false;
                }

                // Set new target
                if (currentTarget != null && canPull)
                {
                    HookPoint hookShow = currentTarget.GetComponent<HookPoint>();
                    if (hookShow != null)
                        hookShow.isAimed = true;

                    Debug.Log("Selected: " + currentTarget.name);
                }
                else {
                    HookPoint hookShow = currentTarget.GetComponent<HookPoint>();
                    if (hookShow != null)
                        hookShow.isAimed = false;
                }

                    previousTarget = currentTarget;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            canJump = true;
            animator.SetBool("isJump", false);
            animator2.SetBool("isJump", false);
        }
    }

    GameObject FindClosestHighest()
    {
        GameObject[] candidates = GameObject.FindGameObjectsWithTag(targetTag);
        GameObject bestTarget = null;

        float minXDistance = Mathf.Infinity;
        float maxY = -Mathf.Infinity;
        Vector3 origin = transform.position;

        foreach (GameObject obj in candidates)
        {
            Vector3 pos = obj.transform.position;

            //Skip objects below the player
            if (pos.y < origin.y)
                continue;

            float distance = Vector3.Distance(origin, pos);

            if (distance <= searchRadius)
            {
                float xDiff = Mathf.Abs(pos.x - origin.x);

                if (xDiff < minXDistance || (Mathf.Approximately(xDiff, minXDistance) && pos.y > maxY))
                {
                    minXDistance = xDiff;
                    maxY = pos.y;
                    bestTarget = obj;
                }
            }
        }

        return bestTarget;
    }
    private System.Collections.IEnumerator EnableMovementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerStats.canMove = true;
    }

    IEnumerator HookCD()
    {
        canPull = false;
        yield return new WaitForSeconds(hookCd);
        canPull = true;
    }






}


