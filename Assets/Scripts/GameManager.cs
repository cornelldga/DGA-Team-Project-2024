using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private int gameTimer;
    private Player player;
    private int numCustomers;
    private int completedOrders;

    private void Awake()
    {
        Instance = this;
        GameManager gameManager = GameManager.Instance;
    }

    public void Start()
    {
        player = FindObjectOfType<Player>();
    }


    public Player getPlayer()
    {
        return player;
    }
    public void addCustomer()
    {

    }
    public void CompleteOrder()
    {

    }
    public void WinGame()
    {

    }
    public void LoseGame()
    {

    }
}
