using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = false; // Allow rotation from physics
    }

    void Update()
    {
        Vector2 velocity = rb.linearVelocity;

        if (velocity.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;

            // Check if mostly moving vertically (or diagonally)
            if (Mathf.Abs(velocity.y) > Mathf.Abs(velocity.x))
            {
                transform.rotation = Quaternion.Euler(0, 0, angle);
                spriteRenderer.flipX = false; // reset flip if previously flipped
            }
            else
            {
                // Horizontal motion � flip if going left
                transform.rotation = Quaternion.identity; // don't rotate
                spriteRenderer.flipX = velocity.x < 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
