using UnityEngine;

public class WaterCharger : MonoBehaviour
{
    [SerializeField] bool isInfinite= false; // If true, the water charger can be used infinitely
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            PlayerAttack  player = collision.GetComponent<PlayerAttack>();
            if (player != null)
            {
                player.Recharge();
                if(!isInfinite)
                {
                    // If the charger is not infinite, destroy it after use
                    Destroy(gameObject);
                }
                
            }
        }
    }
}
