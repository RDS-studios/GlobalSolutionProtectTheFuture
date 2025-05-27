using SmallHedge.SoundManager;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    PlayerInput playerInputs;
    PlayerStats playerStats;
    public int ammo = 10; // Example ammo count, adjust as needed
    public float attackCooldown = 0.5f; // Cooldown time between attacks
    [SerializeField] bool canAttack = true; // Flag to check if player can attack
    [SerializeField] GameObject waterJetPrefab;
    void Start()
    {
        playerInputs = GetComponent<PlayerInput>(); 
        playerStats = GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInputs.actions["Attack"].triggered && canAttack && playerStats.hasWaterJet  )
        {
            StartCoroutine(Attack()); // Start attack coroutine if conditions are met
        }
    }


    IEnumerator Attack()
    {
        if (ammo > 0)
        {
            canAttack = false;
            ammo--;
            SoundManager.PlaySound(SoundType.Esguicho); // Play water jet sound
            GameObject proj = Instantiate(waterJetPrefab, transform.localPosition, Quaternion.identity); // Instantiate water jet                                               
            Rigidbody2D rbproj = proj.GetComponent<Rigidbody2D>();
            rbproj.AddForce(new Vector2(transform.localScale.x * 10f, 1.5f), ForceMode2D.Impulse); // Apply force to the projectile

            yield return new WaitForSeconds(attackCooldown); // Wait for cooldown
            canAttack = true; // Reset attack flag
        }
        
    }
}
