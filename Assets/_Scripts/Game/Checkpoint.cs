using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] PlayerStats playerStats; // Reference to PlayerStats script
    [SerializeField] PlayerAttack playerAttack; // Reference to PlayerAttack script
    [SerializeField] GameSaver gameSaver; // Reference to GameSaver script


    [SerializeField] int hpJogador;
    [SerializeField] int moedasColetadas;
    [SerializeField] int ammoJogador;
    [SerializeField] bool hasWaterJet;
    [SerializeField] Transform posCheckpoint;

    [SerializeField]private bool saved = false;


    [SerializeField] Animator animator; // Animator for checkpoint interaction    
    void Start()
    {
        gameSaver = GameObject.FindGameObjectWithTag("GameSaver").GetComponent<GameSaver>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!saved) {
                playerAttack = collision.GetComponent<PlayerAttack>();
                playerStats = collision.GetComponent<PlayerStats>();
                saved = true;
                animator.SetTrigger("save"); // Trigger checkpoint animation
                SaveStatus();

             
            }
        }
    }


    void SaveStatus()
    {
        hpJogador = playerStats.lives; // Save player's lives
        moedasColetadas = playerStats.coins; // Save collected coins
        ammoJogador = playerAttack.ammo; // Save ammo count
        gameSaver.transformCheckpoint = transform; // Save checkpoint position 
        playerStats.moedasColetadas.Clear(); // Clear collected coins list      



        PlayerPrefs.SetInt("PlayerLives", hpJogador);
        PlayerPrefs.SetInt("PlayerCoins", moedasColetadas);
        PlayerPrefs.SetInt("PlayerAmmo", ammoJogador);
        PlayerPrefs.SetInt("HasWaterJet", playerStats.hasWaterJet ? 1 : 0); // Save water jet status

        PlayerPrefs.Save();

        Debug.Log("Checkpoint saved: Lives: " + hpJogador + ", Coins: " + moedasColetadas + ", Ammo: " + playerAttack.ammo);
    }


}
