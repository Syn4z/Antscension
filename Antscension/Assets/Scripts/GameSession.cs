using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int coins = 0;
    [SerializeField] int jumps = 2;
    [SerializeField] int dash = 1;

    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI jumpsText;
    [SerializeField] TextMeshProUGUI dashText;

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
        dashText.text = dash.ToString();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            UpgradeJump();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            UpgradeDash();
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
            FindObjectOfType<PlayerMovement>().UpgradeDash();        SceneManager.LoadScene(currentSceneIndex);
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
            jumps -= 1;
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
        if (coins >= 3)
        {
            coins -= 3;
            extraJumps++;
            coinText.text = coins.ToString();
            FindObjectOfType<PlayerMovement>().UpdateMaxJumps(extraJumps);
 
            jumps++;
            jumpsText.text = jumps.ToString();
        }
        else
        {
            Debug.Log("Not enough coins to upgrade.");
        }
    }

    public void DecrementDash()
    {
        if (dash > 0)
        {
            dash -= 1;
            dashText.text = dash.ToString();
        }
    }

    public void ResetDash()
    {
        dash = 1;
        dashText.text = dash.ToString();
    }

    public void UpgradeDash()
    {
        if (coins >= 3)
        {
            coins -= 3;
            coinText.text = coins.ToString();
            FindObjectOfType<PlayerMovement>().UpgradeDash();
        }
        else
        {
            Debug.Log("Not enough coins to upgrade.");
        }
    }
}