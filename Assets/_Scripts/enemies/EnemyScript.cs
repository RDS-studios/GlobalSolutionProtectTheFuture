using SmallHedge.SoundManager;
using System.Collections;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] bool canMove = false;
    [SerializeField] int direction = 1; // 1 for right, -1 for left
    [SerializeField] Rigidbody2D rb;
    [SerializeField] int enemyHP = 1; // Health points of the enemy
    [SerializeField] float flipSpeed = 5f; // How fast the flip happens
    [SerializeField] Animator animator;

    [SerializeField] bool segredo = false; // porra do segredo, vai tomar no seu cu diego

    [Header("Falling Settings")]
    [SerializeField] bool falling = false;
    [SerializeField] float fallingSpeed = -1f; // constant descent speed

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(Move());
    }

    void Update()
    {
        if (canMove && rb.bodyType != RigidbodyType2D.Static)
        {
            // If falling is enabled, apply constant downward speed
            float verticalVelocity = falling ? fallingSpeed : rb.linearVelocity.y;

            rb.linearVelocity = new Vector2(direction * speed, verticalVelocity);

             
        }

        // Smooth scale flip based on direction
        Vector3 currentScale = transform.localScale;
        float targetX = Mathf.Sign(direction); // 1 or -1
        float newX = Mathf.Lerp(currentScale.x, targetX, Time.deltaTime * flipSpeed);
        transform.localScale = new Vector3(newX, currentScale.y, currentScale.z);
    }

    IEnumerator Move()
    {
        canMove = true;
        direction = Random.value < 0.5f ? -1 : 1;
        yield return new WaitForSeconds(1f);
        canMove = false;
        StartCoroutine(Move());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            ChangeDirection();
        }
    }

    void ChangeDirection()
    {
        direction *= -1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("WaterJet"))
        {
            enemyHP--;
            if (enemyHP <= 0)
            {
                Die(); // Destroy the enemy if HP is 0 or less
            }
        }
    }

    void Die()
    {
        speed = 0f; // Stop movement
        if (animator != null)
        {
            animator.SetTrigger("Die");
            gameObject.layer = LayerMask.NameToLayer("deadenemie"); // Change layer
            SoundManager.PlaySound(SoundType.InimigoMorte); // Play death sound
            Destroy(gameObject, 1.4f);
            
        }
        else
        {
            if (segredo)
            {
                PlayerPrefs.SetInt("savedTu", 1);
            }
            Destroy(gameObject); // If no animator, just destroy immediately
        }
    }
}
