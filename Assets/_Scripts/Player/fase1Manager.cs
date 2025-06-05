using UnityEngine;

public class fase1Manager : MonoBehaviour
{
    [SerializeField] GameObject Npc1;
    [SerializeField] GameObject Npc12;
    [SerializeField] GameObject Npc2;
    [SerializeField] GameObject Npc22;
    [SerializeField] GameObject Npc3;
    [SerializeField] GameObject Npc32;
    [SerializeField] GameObject Npc4;
    [SerializeField] GameObject Npc42;


    [SerializeField] LevelEnder ender;

    public int npcSaved = 0; // Number of NPCs saved


    private void Start()
    {
        ender = GameObject.FindGameObjectWithTag("faseEnder").GetComponent<LevelEnder>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("npc1"))
        {
            Npc1.SetActive(true);
            Npc12.SetActive(true);
            npcSaved++;
            Destroy(collision.gameObject); // Optionally destroy the NPC object after saving
        }
        else if (collision.CompareTag("npc2"))
        {
            Npc2.SetActive(true);
            Npc22.SetActive(true);
            npcSaved++;
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("npc3"))
        {
            Npc3.SetActive(true);
            Npc32.SetActive(true);
            npcSaved++;
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("npc4"))
        {
            Npc4.SetActive(true);
            Npc42.SetActive(true);
            npcSaved++;
            Destroy(collision.gameObject);
        }
       if(npcSaved == 4)
        {
            ender.canEndLevel = true; // Allow the level to end when all NPCs are saved
        }
         
    }
}
