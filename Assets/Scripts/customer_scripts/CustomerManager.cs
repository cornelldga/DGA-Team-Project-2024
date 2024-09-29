using UnityEngine;
using System.Collections.Generic;

public class CustomerManager : MonoBehaviour
{
    public GameObject customerPrefab;
    public Transform spawnLocation; 
    private List<Customer> customers = new List<Customer>();

    // Method to spawn customers
    public void SpawnCustomer(string name, float waitTime, float cookTime, float returnTime)
    {
        GameObject newCustomer = Instantiate(customerPrefab, spawnLocation.position, Quaternion.identity);
        Customer customerScript = newCustomer.GetComponent<Customer>();
        customerScript.Initialize(name, waitTime, cookTime, returnTime);
        customers.Add(customerScript);
    }

    // Cleanup finished customers
    void Update()
    {
        foreach (Customer customer in customers)
        {
            if (customer != null && customer.isOrderCompleted)
            {
                Debug.Log(customer.customerName + " is completed and removed.");
                customers.Remove(customer);
            }
        }
    }

    public void DeliverOrder(Customer customer)
    {
        if (customers.Contains(customer))
        {
            customer.CompleteOrder();
        }
    }
}
