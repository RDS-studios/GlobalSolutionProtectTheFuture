using SmallHedge.SoundManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunction : MonoBehaviour
{
    [SerializeField] 



    public void PlayGame()
    {
        SoundManager.PlaySound(SoundType.BtnMenu);
        SceneManager.LoadScene(1); // Replace "GameScene" with the actual name of your game scene
    }


    public void BackToMenu()
    {
        SoundManager.PlaySound(SoundType.BtnBack);
        // Implement the logic to go back to the main menu
    }

    public void EnterConfig()
    {
        SoundManager.PlaySound(SoundType.BtnMenu);
        // Implement the logic to enter the configuration menu
    }


    public void QuitGame()
    {
        SoundManager.PlaySound(SoundType.BtnBack);
        // Implement the logic to quit the game
        Application.Quit();
    }
}
