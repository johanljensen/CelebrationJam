using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    private bool gameActive = true;

    void Awake()
    {
        // Singleton pattern to maintain one controller
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Game starts immediately
        StartGame();
    }

    void Update()
    {
        // Quick restart with R key
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
        
        // Quick quit with Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    public void StartGame()
    {
        gameActive = true;
        Time.timeScale = 1f;
        // Add any game start logic here
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public void GameOver()
    {
        gameActive = false;
        // Optional: Brief pause before allowing restart
        Invoke("AllowRestart", 1.0f);
    }

    private void AllowRestart()
    {
        Debug.Log("Game Over! Press R to restart or ESC to quit");
    }
}