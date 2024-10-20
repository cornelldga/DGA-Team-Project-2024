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
    [Tooltip("How long the player has to complete the level")]
    [SerializeField] private int gameTimer;
    private Player player;
    private int numCustomers = 0;
    [Tooltip("The minimum number of customers required to win")]
    [SerializeField] private int minCustomersToWin;
    private int completedOrders = 0;

    private void Awake()
    {
        Instance = this;
        GameManager gameManager = GameManager.Instance;
        player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        AddCustomers();
        InvokeRepeating(nameof(UpdateGameTimer), 1, 1);
    }

    /// <summary>
    /// Counts customers in the scene to be used to track orders completed to the number of customers in that level
    /// </summary>
    void AddCustomers()
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
    /// Called when an order is completed 
    /// </summary>
    public void CompleteOrder()
    {
        completedOrders++;
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
