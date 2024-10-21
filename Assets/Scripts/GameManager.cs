using System.Collections;
using System.Collections.Generic;
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
    private int numCustomers = 0;
    [Tooltip("The minimum number of customers required to win")]
    [SerializeField] private int minCustomersToWin;
    private int completedOrders = 0;
    [Space]
    [Header("UI")]
    public HotbarManager hotbarManager;

    private void Awake()
    {
        Instance = this;
        GameManager gameManager = GameManager.Instance;
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if (customers.Count != 0)
        {
            player.HandleOrders(customers);
        }
    }

    private void Start()
    {
        CountCustomers();
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
        if (gameTimer <= 0)
        {
            StopGame();
        }
    }

    void StopGame()
    {
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
    }
    /// <summary>
    /// Called when a customer should be removed from the list of customers
    /// </summary>
    /// <param name="customer"></param>
    public void RemoveOrder(Customer customer) { 
        customers.Remove(customer);
    }

    /// <summary>
    /// Called when an order is completed 
    /// </summary>
    public void CompleteOrder(Customer customer)
    {
        completedOrders++;
        customers.Remove(customer);
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
        Debug.Log("You Win!");
        Time.timeScale = 0;
    }
    /// <summary>
    /// Ends the game and triggers the lose condition for that level.
    /// </summary>
    public void LoseGame()
    {
        Debug.Log("You Lose!");
        Time.timeScale = 0;
    }
}
