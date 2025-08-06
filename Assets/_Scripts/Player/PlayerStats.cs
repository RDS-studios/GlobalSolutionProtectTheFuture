using SmallHedge.SoundManager;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using ToonBoom.Harmony;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public bool immune = false;
    public int lives = 3;
    public int coins = 0;
    public bool canMove = true;
    public bool hasWaterJet = false;

    [SerializeField] List<GameObject> hearts = new List<GameObject>();
    public List<GameObject> moedasColetadas = new List<GameObject>();

    [SerializeField] GameSaver gameSaver;

    [SerializeField] HarmonyRenderer harmonyRendererNoJet;
    [SerializeField] HarmonyRenderer harmonyRendererWithJet;
    [SerializeField] Animator animatorNoJet;
    [SerializeField] Animator animatorWithJet;

    [SerializeField] TMP_Text coinsText;

    [SerializeField] bool isFinalLevel = false;

    void Start()
    {
        canMove = true;
        gameSaver = GameObject.FindGameObjectWithTag("GameSaver").GetComponent<GameSaver>();

        hearts.Clear();
        hearts.Add(GameObject.FindGameObjectWithTag("Heart1"));
        hearts.Add(GameObject.FindGameObjectWithTag("Heart2"));
        hearts.Add(GameObject.FindGameObjectWithTag("Heart3"));

        coinsText = GameObject.FindGameObjectWithTag("txtCoins").GetComponent<TMP_Text>();

        // Load saved stats at start
        lives = PlayerPrefs.GetInt("PlayerLives", 3);
        coins = PlayerPrefs.GetInt("PlayerCoins", 0);
        hasWaterJet = PlayerPrefs.GetInt("HasWaterJet", 0) == 1;

        coinsText.text = coins.ToString();
        UpdateHeartsVisual();
    }

    void Update()
    {
        coinsText.text = coins.ToString();

        if (coins >= 10)
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
            SoundManager.PlaySound(SoundType.Comer);
            coins++;
            collision.GetComponent<SpriteRenderer>().enabled = false;
            collision.tag = "Untagged";
            moedasColetadas.Add(collision.gameObject);
            coinsText.text = coins.ToString();
        }

        if (collision.CompareTag("BossHand") && !immune)
        {
            TakeDmg();
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            float forceX = collision.transform.position.x > transform.position.x ? -3.7f : 3.7f;
            rb.AddForce(new Vector2(forceX, 4f), ForceMode2D.Impulse);
        }

        if (collision.gameObject.tag ==  "water" )
        {
            StartCoroutine(BackToCheckpoint());
        }
    }

    void TakeDmg()
    {
        StartCoroutine(Iframes());

        if (lives > 0)
        {
            SoundManager.PlaySound(SoundType.Dano);
            lives--;
            UpdateHeartsVisual();
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

    public void Heal()
    {
        if (lives < hearts.Count)
        {
            SoundManager.PlaySound(SoundType.Curar);
            lives++;
            UpdateHeartsVisual();
        }
    }

     
        void UpdateHeartsVisual()
        {
            for (int i = 0; i < hearts.Count; i++)
            {
                Animator anim = hearts[i].GetComponent<Animator>();
                if (i < lives)
                    anim.SetBool("visible", true);
                else
                    anim.SetBool("visible", false);
            }
        }

    

    public void BackToCheckPoint()
    {
        StartCoroutine(BackToCheckpoint());
    }

    IEnumerator BackToCheckpoint()
    {
        TakeDmg();
        canMove = false;
        transform.position = new Vector3(gameSaver.transformCheckpoint.position.x, gameSaver.transformCheckpoint.position.y, transform.position.z);
        yield return new WaitForSeconds(2.5f);
        canMove = true;
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

    IEnumerator Die()
    {
        animatorNoJet.SetTrigger("Die");
        animatorWithJet.SetTrigger("Die");
        canMove = false;
        SoundManager.PlaySound(SoundType.Morte);
        gameSaver.CurtinaIn();

        yield return new WaitForSeconds(2.5f);

        animatorNoJet.SetTrigger("revive");
        animatorWithJet.SetTrigger("revive");

        if (gameSaver.transformCheckpoint == null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            yield break;
        }

        // Teleport to checkpoint
        transform.position = new Vector3(gameSaver.transformCheckpoint.position.x, gameSaver.transformCheckpoint.position.y, transform.position.z);

        // Reload saved stats
        lives = PlayerPrefs.GetInt("PlayerLives", 3);
        coins = PlayerPrefs.GetInt("PlayerCoins", 0);
        hasWaterJet = PlayerPrefs.GetInt("HasWaterJet", 0) == 1;

        UpdateHeartsVisual();

        canMove = true;

        if (isFinalLevel)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
