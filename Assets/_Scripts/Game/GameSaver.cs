using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSaver : MonoBehaviour
{
    public Transform transformCheckpoint;
    [SerializeField] PlayerInput action;
    
   
    public bool isPaused;



    [SerializeField] int currentSceneIndex;


    [SerializeField] Animator animatorBlackBGPauseMenu;
    [SerializeField] Animator animatorPauseMenu;
    [SerializeField] GameObject pauseMenuOBJ;
    [SerializeField] GameObject canvasUI;




   
void OnEnable()
{
    SceneManager.sceneLoaded += OnSceneLoaded;
}

void OnDisable()
{
    SceneManager.sceneLoaded -= OnSceneLoaded;
}
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentSceneIndex = scene.buildIndex;

        // Optional: Try to find the canvas again in case it's different in the new scene
        if (canvasUI == null)
        {
            canvasUI = GameObject.FindWithTag("CanvasUI"); // Make sure your Canvas has this tag!
        }

        if (canvasUI != null)
        {
            if (currentSceneIndex < 3)
                canvasUI.SetActive(false);
            else
                canvasUI.SetActive(true);
        }

        Debug.Log("Scene loaded: " + scene.name + " (Index: " + currentSceneIndex + ")");
    }
    private void Awake()
    {
        
    }

    

    void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;


        DontDestroyOnLoad(gameObject); // Garante que o GameSaver não seja destruído ao carregar uma nova cena

        TogglePause();

       



        if (currentSceneIndex < 3)
        {
            canvasUI.SetActive(false);
        }
      
        
        if (currentSceneIndex > 2)
        {
            canvasUI.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {

       if(action.actions["Pause"].triggered && currentSceneIndex >= 3)
        {
            isPaused = !isPaused; // Inverte o estado de pausa
            TogglePause();
        }


        if (isPaused)
        {

            Time.timeScale = 0f; // Pausa o jogo    


        }
        else
        {
            Time.timeScale = 1f; // Continua o jogo}
        }
    }

    void TogglePause()
    {
        if (isPaused)
        {
            //pauseMenuOBJ.SetActive(true);

            animatorBlackBGPauseMenu.SetBool("isPaused", true);
            animatorPauseMenu.SetBool("isPaused", true);
        }
        else if(!isPaused)
        {
          //

            animatorBlackBGPauseMenu.SetBool("isPaused",false);
            animatorPauseMenu.SetBool("isPaused",false);
        }
        
    }


    public void ChangePause()
    {
        isPaused = !isPaused; // Inverte o estado de pausa
        TogglePause();
    }

    public void ExitToLobby()
    {
        ChangePause();
        SceneManager.LoadScene(2); //lobby
    }

    public void ExitToMainMenu()
    {
        ChangePause();
        SceneManager.LoadScene(1); //main menu
    }   

}
