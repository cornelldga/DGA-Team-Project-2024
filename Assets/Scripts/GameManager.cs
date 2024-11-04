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
    [SerializeField] private int gameTimer;
    private Player player;
    private int numCustomers;
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

    private void Awake()
    {
        Instance = this;
        GameManager gameManager = GameManager.Instance;
        player = FindObjectOfType<Player>();
        hotbarManager = FindObjectOfType<HotbarManager>();
    }

    private void Update()
    {
        if (!gameOver)
        {
            if (customers.Count != 0)
            {
                player.HandleOrders(customers);
            }
        }
    }

    private void Start()
    {
        gameTimerText.text = gameTimer.ToString();
        CountCustomers();
        numCustomersText.text = completedOrders.ToString() + "/" + numCustomers.ToString();
        InvokeRepeating(nameof(UpdateGameTimer), 1, 1);
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
    /// Called every 
    /// </summary>
    private void UpdateGameTimer()
    {
        gameTimer--;
        gameTimerText.text = gameTimer.ToString();
        if (gameTimer <= 0)
        {
            StopGame();
        }
    }

    void StopGame()
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
            Debug.Log("minimum customers met!");
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
        winScreen.SetActive(true);
    }
    /// <summary>
    /// Ends the game and triggers the lose condition for that level.
    /// </summary>
    public void LoseGame()
    {
        loseScreen.SetActive(true);
    }
}
