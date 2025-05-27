using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovimentation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] PlayerInput playerInputs;
    [SerializeField] Rigidbody2D rb2d;


    [Header("Movement Settings")]
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] bool canJump = true;   

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        playerInputs = GetComponent<PlayerInput>();

    }

    // Update is called once per frame
    void Update()
    {
       rb2d.linearVelocityX = playerInputs.actions["Move"].ReadValue<Vector2>().x * speed;
        if (canJump && playerInputs.actions["Jump"].triggered)
        {
            rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            canJump = false;
        }


    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Ground"))
        {
            canJump = true;
        }   
    }
}
