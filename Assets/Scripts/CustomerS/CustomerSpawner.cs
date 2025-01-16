using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] CompletedOrder[] completedOrders;
    [SerializeField] GameObject[] customerPrefabs;

    [SerializeField] float timeBetweenOrdersMin = 1;
    [SerializeField] float timeBetweenOrdersMax = 5;
    float nextOrderTime;

    void UpdateNextOrderTime()
    {
        nextOrderTime = Time.time + Random.Range(timeBetweenOrdersMin, timeBetweenOrdersMax);
    }

    bool LogWarningReturn(string warningText)
    {
        Debug.LogWarning(warningText);
        return true;
    }

    void CreateOrder()
    {
        // Warnings
        bool shouldStop = false;

        if (GameManager.instance.orders.Length == 0)
            shouldStop = LogWarningReturn("The Orders array in the GameManager instance does not contain any elements!");

        if (customerPrefabs.Length == 0)
            shouldStop = LogWarningReturn("The CustomerPrefabs array does not contain any elements!");
        else
            if (!customerPrefabs[0].TryGetComponent<OrderRequest>(out OrderRequest temp))
                shouldStop = LogWarningReturn("The CustomerPrefabs do not contain an OrderRequest component!");

        if (completedOrders.Length == 0)
            shouldStop = LogWarningReturn("The CompletedOrders array does not contain any elements!");

        if (shouldStop)
            return;



        // Getting the ordersSlotIndex
        int ordersSlotIndex = -1; // The index of the slot wherein this order will go
        for (int i = 0; i < GameManager.instance.orders.Length; i++)
        {
            if (!GameManager.instance.orders[i] && ordersSlotIndex == -1)
            {
                ordersSlotIndex = i;
            }
        }

        if (ordersSlotIndex == -1) // If the Orders array is already filled with OrderRequests, don't make a new OrderRequest
            return;


        GameObject newCustomer = Instantiate<GameObject>(customerPrefabs[Random.Range(0, customerPrefabs.Length)]);
        OrderRequest newOrder = null;

        if (newCustomer.TryGetComponent<OrderRequest>(out OrderRequest orderRequest))
            newOrder = orderRequest;


        newOrder.order = completedOrders[Random.Range(0, completedOrders.Length)];
        float distance = (3f / GameManager.instance.orders.Length); // Calculates the distance between customers based on the max amount of customers
        newOrder.transform.position = transform.position + Vector3.left * ordersSlotIndex * distance; // Calculates and sets the position of the orders
        newOrder.transform.parent = transform;

        GameManager.instance.orders[ordersSlotIndex] = newOrder; // Puts the order in the Orders array
    }

    void Update()
    {
        if (Time.time >= nextOrderTime) // Checks if should create new order
        {
            CreateOrder();
            UpdateNextOrderTime();
        }
    }
}
