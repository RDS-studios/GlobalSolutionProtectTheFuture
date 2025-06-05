using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerSpecialMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] PlayerInput playerInputs;
    [SerializeField] Animator animator;
    [SerializeField] Animator animator2;
    [SerializeField] PlayerStats playerStats;

    [Header("Movement Settings")]
    [SerializeField] float moveSpeed = 5f;

    [Header("Movement Limits")]
    [SerializeField] float minX = -10f;
    [SerializeField] float maxX = 10f;
    [SerializeField] float minY = -5f;
    [SerializeField] float maxY = 5f;

    private Vector2 moveInput;
    private int facingDirection = 1;

    void Start()
    {
        playerInputs = GetComponent<PlayerInput>();
        playerStats = GetComponent<PlayerStats>();

        StartCoroutine(lvl()); // Start the level coroutine to trigger the curtain animation and load the next scene    
    }


    IEnumerator lvl()
    {
        yield return new WaitForSeconds(45);
        GameSaver gameSaver = GameObject.FindGameObjectWithTag("GameSaver").GetComponent<GameSaver>();
        gameSaver.CurtinaIn(); // Trigger the curtain animation after 50 seconds    
        yield return new WaitForSeconds(  .75f); // Wait for the curtain animation to finish
        SceneManager.LoadScene(6);
    }

    void Update()
    {
        if (!playerStats.canMove) return;

        moveInput = playerInputs.actions["Move"].ReadValue<Vector2>();

        // Move the player using transform
        Vector3 moveDelta = new Vector3(moveInput.x, moveInput.y, 0) * moveSpeed * Time.deltaTime;
        Vector3 newPosition = transform.position + moveDelta;

        // Clamp the position
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        transform.position = newPosition;

        // Handle animations
        bool isMoving = moveInput.magnitude > 0.1f;
        animator.SetBool("isWalk", isMoving);
        animator2.SetBool("isWalk", isMoving);

        // Optionally update facing direction (if needed)
        // Uncomment if you want the sprite to flip again
        /*
        if (moveInput.x > 0)
            facingDirection = 1;
        else if (moveInput.x < 0)
            facingDirection = -1;

        Vector3 scale = transform.localScale;
        scale.x = facingDirection;
        transform.localScale = scale;
        */
    }
}
