using UnityEngine;

public class Segredo : MonoBehaviour
{

    [SerializeField] int segredoNumber;




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {

            PlayerPrefs.SetInt("segredo" + segredoNumber, 1);
            Destroy(gameObject); 
            
        }
    }
}
