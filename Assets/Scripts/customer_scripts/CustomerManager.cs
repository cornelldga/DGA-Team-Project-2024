using UnityEngine;
using System.Collections.Generic;

public class CustomerManager : MonoBehaviour
{
    public List<Customer> customers;

    void Update()
    {
        for (int i = customers.Count - 1; i >= 0; i--)
        {
            if (customers[i] != null && customers[i].IsOrderCompleted())
            {
                customers.RemoveAt(i);
            }
        }
    }
}
