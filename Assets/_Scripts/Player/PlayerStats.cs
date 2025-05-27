using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int lives = 3;
    public int coins = 0;
    public bool canMove = true;
    [SerializeField] GameObject[] hearts;
    public bool hasWaterJet = false;

    void Start()
    {
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (coins >= 50)
        {
            Heal();
            coins = 0; // Reset coins after healing 


        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Enemie")
        {
            TakeDmg();
            if (collision.gameObject.transform.position.x > transform.position.x)
            {
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                rb.AddForce(new Vector2(-3.7f, 4f), ForceMode2D.Impulse);
            }
            else
            {
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                rb.AddForce(new Vector2(-3.7f, 4f), ForceMode2D.Impulse);
                
            }
            
        }

        void TakeDmg()
        {
            lives--;
            if(lives < 0)
            {
                lives = 0;
                //implement death
            }
            hearts[lives].GetComponent<Animator>().SetTrigger("leave");
            canMove = false;
            StartCoroutine(moveCD());
        }


        IEnumerator moveCD()
        {
            yield return new WaitForSeconds(0.4f);
            canMove = true;
        }
    }

    public void Heal()
    {
        if (lives < hearts.Length)
        {
            hearts[lives].GetComponent<Animator>().SetTrigger("enter");
            lives++;
        }
    }
}