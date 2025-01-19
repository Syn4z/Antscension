using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameSession : MonoBehaviour
{
    private Color greenColor = new Color(201f / 255f, 255f / 255f, 67f / 255f, 1f); 
    private Color redColor = new Color(255f / 255f, 94f / 255f, 66f / 255f, 1f);

    [SerializeField] int playerLives = 3;
    [SerializeField] int coins = 0;
    [SerializeField] int jumps = 2;
    [SerializeField] int dash = 1;

    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI jumpsText;
    [SerializeField] TextMeshProUGUI dashText;
    [SerializeField] TextMeshProUGUI upgradeText;
    [SerializeField] AudioSource upgradeSound;
    [SerializeField] AudioSource failSound;

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
        upgradeText.text = "";
        upgradeText.color = greenColor;
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

            upgradeText.color = greenColor;
            upgradeText.text = "Jump count upgraded!";
            upgradeSound.Play();
            StartCoroutine(ClearMessageAfterDelay(3f));
 
            jumps++;
            jumpsText.text = jumps.ToString();
        }
        else
        {
            Debug.Log("Not enough coins to upgrade.");
            upgradeText.color = redColor;
            upgradeText.text = "Not enough coins \n(3 required)";
            failSound.Play();
            StartCoroutine(ClearMessageAfterDelay(3f));
        }
    }

    private IEnumerator ClearMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        upgradeText.text = "";
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

            upgradeText.color = greenColor;
            upgradeText.text = "Dash distance upgraded!";
            upgradeSound.Play();
            StartCoroutine(ClearMessageAfterDelay(3f));
        }
        else
        {
            Debug.Log("Not enough coins to upgrade.");
            upgradeText.color = redColor;
            upgradeText.text = "Not enough coins \n(3 required)";
            failSound.Play();
            StartCoroutine(ClearMessageAfterDelay(3f));
        }
    }
}