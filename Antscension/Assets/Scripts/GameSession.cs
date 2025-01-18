using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int coins = 0;
    [SerializeField] int jumps = 2;

    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI jumpsText;

    [SerializeField] int extraJumps = 0; // New variable to track extra jumps

    void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        livesText.text = playerLives.ToString();
        coinText.text = coins.ToString();
        jumpsText.text = jumps.ToString();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            UpgradeJump();
        }
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    public void AddToCoins(int coinsToAdd)
    {
        coins += coinsToAdd;
        coinText.text = coins.ToString();
    }

    void TakeLife()
    {
        playerLives--;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        livesText.text = playerLives.ToString();
    }

    void ResetGameSession()
    {
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    public void DecrementJumps()
    {
        if (jumps > 0)
        {
            Debug.Log("Jumps initial: " + jumps);
            jumps -= 1;
            Debug.Log("Jumps decremented to: " + jumps);
            jumpsText.text = jumps.ToString();
        }
    }

    public void ResetJumps()
    {
        jumps = 2 + extraJumps;
        jumpsText.text = jumps.ToString();
        //Debug.Log("Jumps reset to: " + jumps);
    }

    public void UpgradeJump()
    {
        Debug.Log(coins);
        Debug.Log("Button linked to: " + gameObject.name);
        if (coins >= 3)
        {
            coins -= 3;
            extraJumps++;
            coinText.text = coins.ToString();
            FindObjectOfType<PlayerMovement>().UpdateMaxJumps(extraJumps);
 
            jumps++;
            jumpsText.text = jumps.ToString();
            
            Debug.Log("Upgraded! Extra jumps: " + extraJumps);
        }
        else
        {
            Debug.Log("Not enough coins to upgrade.");
        }
    }
}