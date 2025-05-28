using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public bool immune = false; // Player is immune to damage   
    public int lives = 3;
    public int coins = 0;
    public bool canMove = true;
    [SerializeField] GameObject[] hearts;
    public bool hasWaterJet = false;



    public List<GameObject> moedasColetadas = new List<GameObject>();

    


    [SerializeField] GameSaver gameSaver;

    void Start()
    {
        canMove = true;
        gameSaver = GameObject.FindGameObjectWithTag("GameSaver").GetComponent<GameSaver>();
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
            if (!immune) // Check if player is not immune to damage
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
        }



    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            coins++;
            collision.GetComponent<SpriteRenderer>().enabled = false; // Hide the coin sprite
            collision.tag = "Untagged"; // Change tag to prevent further collection
            moedasColetadas.Add(collision.gameObject);
          
        }
       
    }


    void TakeDmg()
        {
            StartCoroutine(Iframes()); // Start invincibility frames    
            if (lives > 0)
            {
                lives--;
                hearts[lives].GetComponent<Animator>().SetTrigger("leave");

            }

            if (lives <= 0)
            {
                StartCoroutine(Die());
            foreach (GameObject moeda in moedasColetadas)
            {
                moeda.GetComponent<SpriteRenderer>().enabled = true; // Hide collected coins
                moeda.tag = "Coin";
            }
             

            }
            canMove = false;
            StartCoroutine(moveCD());
        }
        IEnumerator Iframes()
        {
            immune = true;
            yield return new WaitForSeconds(0.5f);
            immune = false; 

        }

        IEnumerator moveCD()
        {
            yield return new WaitForSeconds(0.4f);
            canMove = true;
        }
    

    public void Heal()
    {
        if (lives < hearts.Length)
        {
            hearts[lives].GetComponent<Animator>().SetTrigger("enter");
            lives++;
        }
    }




    IEnumerator Die()
    {
        
        canMove = false;
        transform.position = new Vector3(gameSaver.transformCheckpoint.position.x, gameSaver.transformCheckpoint.position.y, transform.position.z);
        yield return new WaitForSeconds(2.5f);
        canMove = true;
        // Reset heart visuals
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].GetComponent<Animator>().SetTrigger("enter");
        }

        // Restore lives and other saved stats
        lives = PlayerPrefs.GetInt("PlayerLives");
        coins = PlayerPrefs.GetInt("PlayerCoins");
        hasWaterJet = PlayerPrefs.GetInt("HasWaterJet") == 1;

        // Apply actual heart count (hide the extras if needed)
        for (int i = lives; i < hearts.Length; i++)
        {
            hearts[i].GetComponent<Animator>().SetTrigger("leave");
        }










    }
} 