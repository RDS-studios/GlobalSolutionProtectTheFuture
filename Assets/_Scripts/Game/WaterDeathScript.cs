using UnityEngine;

public class WaterDeathScript : MonoBehaviour
{
     

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ("Player"))
        {
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();


            playerStats.BackToCheckPoint(); // Call the Die method to handle player death

        }
    }
}
