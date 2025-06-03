using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnder : MonoBehaviour
{
   [SerializeField] PlayerStats playerStats; // Reference to PlayerStats script
    [SerializeField] PlayerAttack playerAttack;
    [SerializeField] int fase;
    [SerializeField] bool canEndLevel = false; // Flag to check if the level can end

    private void Start()
    {
        fase = SceneManager.GetActiveScene().buildIndex; // Get the current scene index
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && canEndLevel)
        {
          
            // Check if the player has reached the end of the level
            Debug.Log("Player has reached the end of the level.");
            playerAttack = collision.GetComponent<PlayerAttack>();
            playerStats = collision.GetComponent<PlayerStats>();
              
            
            playerStats.moedasColetadas.Clear(); // Clear collected coins list      



            PlayerPrefs.SetInt("PlayerLives", playerStats.lives);
            PlayerPrefs.SetInt("PlayerCoins", playerStats.coins);
            PlayerPrefs.SetInt("PlayerAmmo", playerAttack.ammo);
            PlayerPrefs.SetInt("HasWaterJet", playerStats.hasWaterJet ? 1 : 0); // Save water jet status

            PlayerPrefs.Save();
            // Here you can add logic to handle what happens when the player reaches the end
            // For example, you might want to load a new scene or display a message
            // SceneManager.LoadScene("NextLevel"); // Uncomment this line to load the next level
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Load the next scene in the build order

        }
    }
}
