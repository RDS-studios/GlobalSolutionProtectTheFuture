using SmallHedge.SoundManager;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    [SerializeField] SoundType soundType;
    bool isTriggered = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTriggered)
        {
            SoundManager.PlaySound(soundType);
            isTriggered = true; // esse caralho so toca uma vez agora
        }
    }
}
