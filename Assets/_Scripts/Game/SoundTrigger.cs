using SmallHedge.SoundManager;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    [SerializeField] SoundType soundType;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SoundManager.PlaySound(soundType);
        }
    }
}
