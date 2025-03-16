using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI killCountText;
    
    [Header("Settings")]
    [SerializeField] private int pointsPerKill = 100;
    [SerializeField] private bool showKillCounter = true;
    
    // Private variables
    private int currentScore = 0;
    private int killCount = 0;
    
    // Singleton instance
    public static ScoreManager Instance { get; private set; }
    
    private void Awake()
    {
        // Set up singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        
        // Optional: Make this persist between scenes
        // DontDestroyOnLoad(gameObject);
        
        UpdateUI();
    }
    
    // Call this method when a kill is registered
    public void AddKill()
    {
        killCount++;
        AddScore(pointsPerKill);
        UpdateUI();
    }
    
    // Add custom score amount (for various achievements, etc.)
    public void AddScore(int points)
    {
        currentScore += points;
        UpdateUI();
    }
    
    // Reset counters (useful for game restart)
    public void ResetCounters()
    {
        currentScore = 0;
        killCount = 0;
        UpdateUI();
    }
    
    // Update UI elements with current values
    private void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {currentScore}";
        }
        
        if (killCountText != null && showKillCounter)
        {
            killCountText.text = $"Kills: {killCount}";
        }
        else if (killCountText != null)
        {
            killCountText.gameObject.SetActive(false);
        }
    }
    
    // Getter methods
    public int GetCurrentScore() => currentScore;
    public int GetKillCount() => killCount;
}