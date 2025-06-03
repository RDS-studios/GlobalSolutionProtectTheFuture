using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FaseSelector : MonoBehaviour
{
    [SerializeField] int faseIndex = 0;
    [SerializeField] bool hasUnlockedLevel;
    [SerializeField] GameObject lockedIcon;

    void Start()
    {
        if (faseIndex == 3)
        {
            hasUnlockedLevel = true; // Level 3 is always unlocked
        }
        else if (faseIndex == 4)
        {
            hasUnlockedLevel = PlayerPrefs.GetInt("fase4Unlocked", 0) == 1;

        }
        else if (faseIndex == 5)
        {


            hasUnlockedLevel = PlayerPrefs.GetInt("fase5Unlocked", 0) == 1;



        }
        if (hasUnlockedLevel)
        {
            lockedIcon.SetActive(false); // Hide the locked icon if the level is unlocked
        }
        else
        {
            lockedIcon.SetActive(true); // Show the locked icon if the level is still locked

        }

        // Update is called once per frame
      
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (hasUnlockedLevel)
            {
                PlayerPrefs.SetInt("SelectedFase", faseIndex); // Save the selected level index
                Debug.Log("Fase " + faseIndex + " selecionada.");
                SceneManager.LoadScene(faseIndex); // Load the selected level   
            }
            else
            {
                Debug.Log("Fase " + faseIndex + " ainda está bloqueada.");
            }
        }
    }
}