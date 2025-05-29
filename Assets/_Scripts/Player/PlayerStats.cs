using System.Collections;
using System.Collections.Generic;
using ToonBoom.Harmony;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public bool immune = false; // Player is immune to damage   
    public int lives = 3;
    public int coins = 0;
    public bool canMove = true;
    [SerializeField] List<GameObject> hearts = new List<GameObject>();
    public bool hasWaterJet = false;

    public List<GameObject> moedasColetadas = new List<GameObject>();

    [SerializeField] GameSaver gameSaver;

    [SerializeField] HarmonyRenderer harmonyRendererNoJet;
    [SerializeField] HarmonyRenderer harmonyRendererWithJet;

    void Start()
    {
        canMove = true;
        gameSaver = GameObject.FindGameObjectWithTag("GameSaver").GetComponent<GameSaver>();
        hearts.Clear();

        hearts.Add(GameObject.FindGameObjectWithTag("Heart1"));
        hearts.Add(GameObject.FindGameObjectWithTag("Heart2"));
        hearts.Add(GameObject.FindGameObjectWithTag("Heart3"));
    }

    void Update()
    {
        if (coins >= 50)
        {
            Heal();
            coins = 0;
        }

        harmonyRendererNoJet.enabled = !hasWaterJet;
        harmonyRendererWithJet.enabled = hasWaterJet;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemie") && !immune)
        {
            TakeDmg();

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            float forceX = collision.transform.position.x > transform.position.x ? -3.7f : 3.7f;
            rb.AddForce(new Vector2(forceX, 4f), ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            coins++;
            collision.GetComponent<SpriteRenderer>().enabled = false;
            collision.tag = "Untagged";
            moedasColetadas.Add(collision.gameObject);
        }
    }

    void TakeDmg()
    {
        StartCoroutine(Iframes());

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
                moeda.GetComponent<SpriteRenderer>().enabled = true;
                moeda.tag = "Coin";
            }
        }

        canMove = false;
        StartCoroutine(MoveCooldown());
    }

    IEnumerator Iframes()
    {
        immune = true;
        yield return new WaitForSeconds(0.5f);
        immune = false;
    }

    IEnumerator MoveCooldown()
    {
        yield return new WaitForSeconds(0.4f);
        canMove = true;
    }

    public void Heal()
    {
        if (lives < hearts.Count)
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

        // Restore visuals
        foreach (GameObject heart in hearts)
        {
            heart.GetComponent<Animator>().SetTrigger("enter");
        }

        lives = PlayerPrefs.GetInt("PlayerLives");
        coins = PlayerPrefs.GetInt("PlayerCoins");
        hasWaterJet = PlayerPrefs.GetInt("HasWaterJet") == 1;

        for (int i = lives; i < hearts.Count; i++)
        {
            hearts[i].GetComponent<Animator>().SetTrigger("leave");
        }
    }
}
