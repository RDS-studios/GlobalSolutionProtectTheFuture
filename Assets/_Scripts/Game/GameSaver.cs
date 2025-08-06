
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameSaver : MonoBehaviour
{
    public Transform transformCheckpoint;
    [SerializeField] PlayerInput action;

    [SerializeField] Animator animatorCortina;

     
    [SerializeField] FaseSelector faseSelector2;
    [SerializeField] FaseSelector faseSelector3;



    public bool isPaused;
    public static GameSaver Instance;

    [SerializeField] int currentSceneIndex;

    [SerializeField] Animator animatorBlackBGPauseMenu;
    [SerializeField] Animator animatorPauseMenu;
    [SerializeField] GameObject pauseMenuOBJ;
    [SerializeField] GameObject canvasUI;
    [SerializeField] GameObject canvasConfig;

    //void OnEnable()
    //{
    //    SceneManager.sceneLoaded += OnSceneLoaded;
    //}

    //void OnDisable()
    //{
    //    SceneManager.sceneLoaded -= OnSceneLoaded;
    //}

    private void Awake()
    {
      
    }

    void Start()
    {

        Time.timeScale = 1f;
        isPaused = false;

        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        TogglePause();
        

        if (canvasUI == null)
        {
            canvasUI = GameObject.FindWithTag("CanvasUI");
        }

        canvasUI?.SetActive(currentSceneIndex >= 2);

        
           

        


        LoadState(); //   Carrega os dados do jogador no início
    }



    public void CurtinaIn()
    {
        animatorCortina.SetTrigger("courtain");
    }

    void Update()
    {
        if (action != null && action.actions["Pause"].triggered || Input.GetButtonDown("Cancel") && currentSceneIndex >= 3)
        {
            print("pausando");
            isPaused = !isPaused;
            TogglePause();
        }

        Time.timeScale = isPaused ? 0f : 1f;
    }

    void TogglePause()
    {
        if (animatorBlackBGPauseMenu != null)
            animatorBlackBGPauseMenu.SetBool("isPaused", isPaused);

        if (animatorPauseMenu != null)
            animatorPauseMenu.SetBool("isPaused", isPaused);

        if(isPaused == false)
        {
            canvasConfig.SetActive(false);  
        }
    }

    public void ChangePause()
    {
        isPaused = !isPaused;
        TogglePause();
    }

    public void ExitToLobby()
    {
        SaveState(); //   Salva antes de trocar de cena
        ChangePause();
        SceneManager.LoadScene(15); // Lobby
    }

    public void ExitToMainMenu()
    {
        SaveState(); //   Salva antes de trocar de cena
        ChangePause();
        SceneManager.LoadScene(1); // Main Menu
    }

     

    //   SALVAR O JOGO
    public void SaveState()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        var stats = player.GetComponent<PlayerStats>();
        var attack = player.GetComponent<PlayerAttack>();

        PlayerPrefs.SetInt("PlayerCoins", stats.coins);
        PlayerPrefs.SetInt("PlayerLives", stats.lives);
        PlayerPrefs.SetInt("PlayerAmmo", attack.ammo);
        PlayerPrefs.SetInt("HasWaterJet", stats.hasWaterJet ? 1 : 0);

        PlayerPrefs.Save();
        Debug.Log("Jogo salvo com sucesso.");
    }

    //   CARREGAR O JOGO
    public void LoadState()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        var stats = player.GetComponent<PlayerStats>();
        var attack = player.GetComponent<PlayerAttack>();

        stats.coins = PlayerPrefs.GetInt("PlayerCoins", 0);
        stats.lives = PlayerPrefs.GetInt("PlayerLives", 3);
        attack.ammo = PlayerPrefs.GetInt("PlayerAmmo", 0);
        stats.hasWaterJet = PlayerPrefs.GetInt("HasWaterJet", 0) == 1;

        Debug.Log("Jogo carregado.");
        int ammo = PlayerPrefs.GetInt("PlayerAmmo", 0);
        int hasWaterJet = PlayerPrefs.GetInt("HasWaterJet", 0); // 0 ou 1   

        Debug.Log($"Ammo: {ammo}, HasWaterJet: {hasWaterJet}");
        
        
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentSceneIndex == 6) // Fase 3 completed, unlock Fase 4
        {
            PlayerPrefs.SetInt("fase4Unlocked", 1);
            Debug.Log("Fase 4 desbloqueada!");
        }
        else if (currentSceneIndex == 9) // Fase 4 completed, unlock Fase 5
        {
            PlayerPrefs.SetInt("fase5Unlocked", 1);
            Debug.Log("Fase 5 desbloqueada!");
        }



        if(currentSceneIndex == 15)
        {
            int fase4Unlocked = PlayerPrefs.GetInt("fase4Unlocked", 0);
            int fase5Unlocked = PlayerPrefs.GetInt("fase5Unlocked", 0); 
            if (fase4Unlocked == 1)
            {
                Debug.Log("Desbloqueando Fase 4 no seletor de fases.");
                faseSelector2.hasUnlockedLevel = true;
            }


            if(fase5Unlocked == 1)
            {
                Debug.Log("Desbloqueando Fase 5 no seletor de fases.");
                faseSelector3.hasUnlockedLevel = true;
            }
        }

        PlayerPrefs.Save(); //  Save immediately
    }
}
