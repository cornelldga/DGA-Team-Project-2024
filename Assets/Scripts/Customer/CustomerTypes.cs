using System;
using UnityEngine;


[System.Serializable]
public class CustomerType
{
    public string name;
    public RuntimeAnimatorController animatorController;
}

public class CustomerTypes : MonoBehaviour
{
    public CustomerType[] customerTypes;
    public Customer customer;

    [SerializeField] Animator animator;
    private CustomerType currentCustomerType;

    void Start()
    {
        if (customerTypes.Length != 0 && animator != null)
        {
            Debug.Log(customerTypes);
            string name = customer.customerName;
            bool foundname = false;
            // check if name is in customer types
            foreach (CustomerType type in customerTypes)
            {
                if (type.name == name)
                {
                    currentCustomerType = type;
                }
            }
            if (!foundname)
            {
                Debug.LogError("Customer Type " + name + " Not Found!! Check your customer Name in Customer script!");
                throw new Exception("CUSTOMER TYPE NOT FOUND");
            }
        }
    }

    // Public methods to retrieve sound clips for this pedestrian type
    public string GetRandomOrderCompleteSound()
    {
        return "sfx_complete_" + currentCustomerType.name;
    }

    public string GetRandomHurtSound()
    {
        return "sfx_hurt_" + currentCustomerType.name;
    }

    public string GetRandomTakeOrderSound()
    {
        return "sfx_order_" + currentCustomerType.name;
    }

    private AudioClip GetRandomClip(AudioClip[] clips)
    {
        if (clips != null && clips.Length > 0)
        {
            return clips[UnityEngine.Random.Range(0, clips.Length)];
        }
        return null; // Return null if no clips are found (Handle this in sound manager)
    }
}