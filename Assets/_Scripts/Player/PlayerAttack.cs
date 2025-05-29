using SmallHedge.SoundManager;
using System.Collections;
using ToonBoom.Harmony;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    PlayerInput playerInputs;
    PlayerStats playerStats;
    public int ammo = 10; // Example ammo count, adjust as needed
    public float attackCooldown = 0.5f; // Cooldown time between attacks
    [SerializeField] bool canAttack = true; // Flag to check if player can attack
    [SerializeField] Transform spawnPos;


    [SerializeField] GameObject waterJetPrefab;

    [SerializeField] GameSaver gameSaver; // Reference to GameSaver for saving state






    void Start()
    {
        playerInputs = GetComponent<PlayerInput>(); 
        playerStats = GetComponent<PlayerStats>();
        gameSaver = GameObject.FindGameObjectWithTag("GameSaver").GetComponent<GameSaver>(); // Get GameSaver component 
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInputs.actions["Attack"].triggered && canAttack && playerStats.hasWaterJet  && !gameSaver.isPaused)
        {
            StartCoroutine(Attack()); // Start attack coroutine if conditions are met
        }

        if(ammo == 0)
        {
            playerStats.hasWaterJet = false; // Disable water jet if ammo is zero
           
        }
    
    }


    IEnumerator Attack()
    {
        if (ammo > 0)
        {
            canAttack = false;
            ammo--;
            SoundManager.PlaySound(SoundType.Esguicho); // Play water jet sound
            GameObject proj = Instantiate(waterJetPrefab, spawnPos.position, Quaternion.identity); // Instantiate water jet                                               
            Rigidbody2D rbproj = proj.GetComponent<Rigidbody2D>();
            rbproj.AddForce(new Vector2(transform.localScale.x * 10f, 3.5f), ForceMode2D.Impulse); // Apply force to the projectile

            yield return new WaitForSeconds(attackCooldown); // Wait for cooldown
            canAttack = true; // Reset attack flag
        }
        
    }
}
