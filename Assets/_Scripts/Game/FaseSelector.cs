using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FaseSelector : MonoBehaviour
{
    [SerializeField] int faseIndex = 0;
    public bool hasUnlockedLevel;
    [SerializeField] GameObject lockedIcon;

    

        //void Start()
        //{
        //    hasUnlockedLevel = faseIndex switch
        //    {
        //        3 => true, // Always unlocked
        //        4 => PlayerPrefs.GetInt("fase4Unlocked", 0) == 1,
        //        5 => PlayerPrefs.GetInt("fase5Unlocked", 0) == 1,
        //        _ => false
        //    };

        //   
        //}

     


    private void Update()
    {

        lockedIcon.SetActive(!hasUnlockedLevel);
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