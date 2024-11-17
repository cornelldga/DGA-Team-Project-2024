using System.Collections;
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
    [Tooltip("How long the player has to complete the level")]
    [SerializeField] private float gameTimer;
    [SerializeField] private GameObject pauseMenu;
    private Player player;
    private int numCustomers;
    bool pauseGame = false;
    bool gameOver = false;

    [Tooltip("The minimum number of customers required to win")]
    [SerializeField] private int minCustomersToWin;
    private int completedOrders = 0;
    [Space]
    [Header("GameManager UI")]
    private HotbarManager hotbarManager;
    [SerializeField] TMP_Text gameTimerText;
    [SerializeField] TMP_Text numCustomersText;
    [SerializeField] GameObject loseScreen;
    [SerializeField] GameObject winScreen;

    [Space]
    [Header("GameManager Inputs")]
    [SerializeField] KeyCode pauseKey;

    private void Awake()
    {
        Instance = this;
        GameManager gameManager = GameManager.Instance;
        player = FindObjectOfType<Player>();
        hotbarManager = FindObjectOfType<HotbarManager>();
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
        gameTimerText.text = gameTimer.ToString("F2");
        if (gameTimer <= 0)
        {
            EndGame();
        }
    }
    /// <summary>
    /// Pauses the game, setting timeScale to 0 and disables the player controller and customers
    /// </summary>
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        pauseGame = true;
        player.enabled = false;
        Time.timeScale = 0;
        foreach(Customer customer in customers)
        {
            customer.enabled = false;
        }
    }
    /// <summary>
    /// Resumes the game, setting timeScale to 1 and enables the player controller
    /// </summary>
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        pauseGame = false;
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
        if (completedOrders >= minCustomersToWin)
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
        hotbarManager.AddToHotbar(customer);
    }
    /// <summary>
    /// Called when a customer should be removed from the list of customers
    /// </summary>
    /// <param name="customer"></param>
    public void RemoveOrder(Customer customer) { 
        customers.Remove(customer);
        hotbarManager.RemoveFromHotBar(customer);
    }

    /// <summary>
    /// Called when an order is completed 
    /// </summary>
    public void CompleteOrder(Customer customer)
    {
        completedOrders++;
        numCustomersText.text = completedOrders.ToString() + "/" + numCustomers.ToString();
        if (completedOrders == minCustomersToWin) {
            //indicator that the minimum amount of customers you must serve to win has been fufilled
            numCustomersText.color = Color.green;
        }
        //customers.Remove(customer);
        RemoveOrder(customer);
        if (completedOrders == numCustomers)
        {
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
        winScreen.SetActive(true);
    }
    /// <summary>
    /// Ends the game and triggers the lose condition for that level.
    /// </summary>
    public void LoseGame()
    {
        gameOver = true;
        PauseGame();
        loseScreen.SetActive(true);
    }
}
