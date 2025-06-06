using SmallHedge.SoundManager;
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
    [SerializeField] HookScript hookScript; // Reference to the HookScript for pulling objects
    [SerializeField] Animator hookAnimator;

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
           StartCoroutine(Hook());
        }



        if (true)
        {
            GameObject currentTarget = FindClosestHighest();

            // Reset previous target's aiming visuals if it exists and is different from currentTarget
            if (previousTarget != null && previousTarget != currentTarget)
            {
                HookPoint previousHookPoint = previousTarget.GetComponent<HookPoint>();
                if (previousHookPoint != null)
                    previousHookPoint.isAimed = false;
            }

            // Set new target's aiming visuals if target exists and canPull is true
            if (currentTarget != null && canPull)
            {
                HookPoint currentHookPoint = currentTarget.GetComponent<HookPoint>();
                if (currentHookPoint != null)
                    currentHookPoint.isAimed = true;
            }
            else if (currentTarget != null) // if can't pull, remove aiming
            {
                HookPoint currentHookPoint = currentTarget.GetComponent<HookPoint>();
                if (currentHookPoint != null)
                    currentHookPoint.isAimed = false;
            }

            // Update previousTarget every frame regardless
            previousTarget = currentTarget;
        }
    }

    IEnumerator Hook()
    {
        hookAnimator.SetTrigger("enter");
        yield return new WaitForSeconds(0.2f);
        GameObject currentTarget = FindClosestHighest();

        if (currentTarget != null)
        {
            StartCoroutine(HookCD());
            SoundManager.PlaySound(SoundType.Gancho);
            Vector2 direction = (currentTarget.transform.position - transform.position).normalized;

            HookPoint hookShow = currentTarget.GetComponent<HookPoint>();
            if (hookShow != null)
                hookShow.isAimed = false;

            rb2d.linearVelocity = direction * pullStrength;

            // Start the hook movement coroutine
            StartCoroutine(MoveHookToTarget(currentTarget.transform.position));
            playerStats.canMove = false;
            StartCoroutine(EnableMovementAfterDelay(0.6f));
           
        }
    }





    private IEnumerator MoveHookToTarget(Vector3 worldTargetPos)
    {
        float hookSpeed = 90f;
        Transform hookTransform = hookScript.targetTransform;

        // Store original local position and parent before unparenting
        Vector3 originalLocalPos = hookTransform.localPosition;
        Transform originalParent = hookTransform.parent;

        // Unparent to move freely in world space
        hookTransform.parent = null;

        // Move to target position in world space
        while (Vector3.Distance(hookTransform.position, worldTargetPos) > 0.1f)
        {
            hookTransform.position = Vector3.MoveTowards(
                hookTransform.position,
                worldTargetPos,
                hookSpeed * Time.deltaTime
            );
            yield return null;
        }
        hookTransform.position = worldTargetPos;

        yield return new WaitForSeconds(0.6f);

        // Move back to original world position
        Vector3 originalWorldPos = originalParent.TransformPoint(originalLocalPos); // convert local to world
        while (Vector3.Distance(hookTransform.position, originalWorldPos) > 0.1f)
        {
            hookTransform.position = Vector3.MoveTowards(
                hookTransform.position,
                originalWorldPos,
                hookSpeed * Time.deltaTime
            );
            yield return null;
        }

        // Snap to exact original world position
        hookTransform.position = originalWorldPos;

        // Reparent back to original parent
        hookTransform.parent = originalParent;

        // Reset localPosition relative to parent to avoid offset
        hookTransform.localPosition = originalLocalPos;
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


