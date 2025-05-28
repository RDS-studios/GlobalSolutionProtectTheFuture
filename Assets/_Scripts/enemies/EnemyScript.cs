using System.Collections;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] bool canMove = false;
    [SerializeField] int direction = 1; // 1 for right, -1 for left
    [SerializeField] Rigidbody2D rb;

    [SerializeField] float flipSpeed = 5f; // How fast the flip happens

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(Move());
    }

    void Update()
    {
        if (canMove)
        {
            rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
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
           
        
            Destroy(gameObject); 
          
        }
    }
}
