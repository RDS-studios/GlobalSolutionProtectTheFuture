using UnityEngine;

public class GameSaver : MonoBehaviour
{
    public Transform transformCheckpoint;

    public bool isPaused;



    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused)
        {

            Time.timeScale = 0f; // Pausa o jogo    


        }
        else
        {
            Time.timeScale = 1f; // Continua o jogo}
        }
    }
}
