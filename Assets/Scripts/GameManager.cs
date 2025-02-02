using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class is used to manage the game logic and game state in each level.
/// Controls win and lose conditions and provides reference to the Player
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    //The customers the player is currently handling
    List<Customer> customers = new List<Customer>();

    [Header("Game Logic")]
    [Tooltip("Determines what levels should be unlocked when completing this level")]
    [SerializeField] int levelNumber = -1;
    [Tooltip("How long the player has to complete the level")]
    [SerializeField] private float gameTimer;
    private Player player;
    private int numCustomers;
    bool pauseGame = false;
    bool gameOver = false;
    
    [Tooltip("The minimum number of customers required to win and earn 1 star")]
    [SerializeField] private int oneStarRating;
    [Tooltip("The minimum number of customers required to earn 2 stars")]
    [SerializeField] private int twoStarRating;
    private int completedOrders = 0;
    [Space]
    [Header("GameManager UI")]
    private CookBarManager cookBarManager;
    [SerializeField] TMP_Text gameTimerText;
    [SerializeField] TMP_Text numCustomersText;
    [SerializeField] GameObject gameCanvas;
    [SerializeField] GameObject loseScreen;
    [SerializeField] GameObject winScreen;
    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject oneStar;
    [SerializeField] GameObject twoStar;
    [SerializeField] GameObject threeStar;

    [Space]
    [Header("GameManager Inputs")]
    [SerializeField] KeyCode pauseKey;

    private void Awake()
    {
        Instance = this;
        GameManager gameManager = GameManager.Instance;
        player = FindObjectOfType<Player>();
        cookBarManager = FindObjectOfType<CookBarManager>();
    }
    /// <summary>
    /// Check if game is paused. Update the game timer and handle orders
    /// </summary>
    private void Update()
    {
        if (gameOver)
        {
            return;
        }
        if (Input.GetKeyDown(pauseKey))
        {
            pauseGame = !pauseGame;
            if (pauseGame)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
        if (pauseGame)
        {
            return;
        }
        UpdateGameTimer();
    }

    private void Start()
    {
        if(levelNumber == -1)
        {
            throw new System.Exception("You must set the level number based on its chronological order." +
                " Set to 0 if test level or endless level");
        }

        gameTimerText.text = gameTimer.ToString();
        CountCustomers();
        numCustomersText.text = completedOrders.ToString() + "/" + numCustomers.ToString();
    }

    /// <summary>
    /// Counts customers in the scene to be used to track orders completed to the number of customers in that level
    /// </summary>
    void CountCustomers()
    {
        Customer[] customers = GameObject.FindObjectsOfType<Customer>();
        numCustomers = customers.Length;
    }

    /// <summary>
    /// Returns the reference to the Player script
    /// </summary>
    public Player getPlayer()
        {
            return player;
        }

    /// <summary>
    /// Returns the list of customers that are being handled in the game
    /// </summary>
    public List<Customer> GetCustomers()
    {
        return customers;
    }

    /// <summary>
    /// Called every 
    /// </summary>
    private void UpdateGameTimer()
    {
        gameTimer-=Time.deltaTime;
        if (gameTimer <= 0)
        {
            gameTimerText.text = "0.00";
            AudioManager.Instance.Play("sfx_TimeUp");
            EndGame();
        }
        gameTimerText.text = gameTimer.ToString("F2");
    }
    /// <summary>
    /// Freezes the game by setting timescale to 0 and disabling key game componenents
    /// </summary>
    public void FreezeGame()
    {
        player.enabled = false;
        Time.timeScale = 0;
    }
    /// <summary>
    /// Toggles the game canvas
    /// </summary>
    public void ToggleGameUI(bool toggle)
    {
        gameCanvas.SetActive(toggle);
    }

    /// <summary>
    /// Pauses the game, setting timeScale to 0 and disables the player controller and customers
    /// </summary>
    public void PauseGame()
    {
        pauseGame = true;
        if(gameOver != true){
            this.pauseScreen.SetActive(true);
        }
        foreach (Customer customer in customers)
        {
            customer.enabled = false;
        }
        FreezeGame();
    }
    /// <summary>
    /// Resumes the game, setting timeScale to 1 and enables the player controller
    /// </summary>
    public void ResumeGame()
    {
        pauseGame = false;
        this.pauseScreen.SetActive(false);
        player.enabled = true;
        Time.timeScale = 1;
        foreach (Customer customer in customers)
        {
            customer.enabled = true;
        }
    }

    /// <summary>
    /// Ends the game, triggering WinGame if the minimum customers served was met,
    /// otherwise trigggering EndGame if it is not met
    /// </summary>
    void EndGame()
    {
        player.enabled = false;
        Time.timeScale = 0;
        if (completedOrders >= oneStarRating)
        {
            WinGame();
        }
        else
        {
            LoseGame();
        }
    }

    /// <summary>
    /// Called when the player takes an order from a customer. Add the customer to the list of customers
    /// </summary>
    public void TakeOrder(Customer customer)
    {
        customers.Add(customer);
        cookBarManager.AddToHotbar(customer);
    }
    /// <summary>
    /// Called when a customer should be removed from the list of customers
    /// </summary>
    /// <param name="customer"></param>
    public void RemoveOrder(Customer customer) { 
        customers.Remove(customer);
        cookBarManager.RemoveFromHotBar(customer);
    }

    /// <summary>
    /// Called when an order is completed 
    /// </summary>
    public void CompleteOrder(Customer customer)
    {
        completedOrders++;
        numCustomersText.text = completedOrders.ToString() + "/" + numCustomers.ToString();
        if (completedOrders == oneStarRating)
        {
            //indicator that the minimum amount of customers you must serve to win has been fufilled
            numCustomersText.color = Color.green;
            oneStar.SetActive(true);
        }
        else if (completedOrders == twoStarRating)
        {
            twoStar.SetActive(true);
        }
        //customers.Remove(customer);
        RemoveOrder(customer);
        if (completedOrders == numCustomers)
        {
            threeStar.SetActive(true);
            WinGame();
        }
    }
    /// <summary>
    /// Ends the game and triggers the win condition for that level.
    /// </summary>
    public void WinGame()
    {
        gameOver = true;
        PauseGame();
        PlayerPrefs.SetInt("Levels Completed", Mathf.Max(PlayerPrefs.GetInt("Levels Completed", 0), levelNumber));
        winScreen.SetActive(true);
        //TODO Add jingle to play when winning
    }
    /// <summary>
    /// Ends the game and triggers the lose condition for that level.
    /// </summary>
    public void LoseGame()
    {
        gameOver = true;
        PauseGame();
        loseScreen.SetActive(true);
        //TODO Add jingle to play when losing
    }
    /// <summary>
    /// Resets the current level by reloading
    /// </summary>
    public void ResetLevel()
    {
        ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    /// <summary>
    /// Loads the scene of the specified scene name
    /// </summary>
    public void LoadScene(string sceneName)
    {
        ResumeGame();
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Goes back to the main menu from the pause screen
    /// </summary>
    public void ReturnToMainMenu() {
        FindObjectOfType<AudioManager>().StopSound("sfx_SirenLong");
        FindObjectOfType<AudioManager>().PlaySound("sfx_MenuClick");
        LoadScene("Main Menu");
    }


}
