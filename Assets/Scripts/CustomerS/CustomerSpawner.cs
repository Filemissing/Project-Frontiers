using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public enum Difficulty
    {
        Easy,
        Intermediate,
        Hard
    }

    public Difficulty currentDifficulty = Difficulty.Easy;

    [SerializeField] GameObject[] customerPrefabs;

    [SerializeField] float timeBetweenOrdersMin = 1;
    [SerializeField] float timeBetweenOrdersMax = 5;
    float nextOrderTime;

    List<CompletedOrder> completedOrders = new List<CompletedOrder>();

    void UpdateNextOrderTime()
    {
        nextOrderTime = Time.time + Random.Range(timeBetweenOrdersMin, timeBetweenOrdersMax);
    }

    int GetCurrentOrders()
    {
        int currentOrders = 0;
        for (int i = 0; i < GameManager.instance.orders.Length; i++)
        {
            if (GameManager.instance.orders[i] != null)
                currentOrders++;
        }
        return currentOrders;
    }

    void SetCompletedOrders()
    {
        for (int i = 0; i < GameManager.instance.recipes.Length; i++)
        {
            if (GameManager.instance.recipes[i].endResult.TryGetComponent<CompletedOrder>(out CompletedOrder component))
                completedOrders.Add(component);
            else
                Debug.Log(GameManager.instance.recipes[i].endResult + " does not have a CompletedOrder component!");
        }
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

        if (completedOrders.Count == 0)
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


        newOrder.order = completedOrders[Random.Range(0, completedOrders.Count)];
        float distance = (3f / GameManager.instance.orders.Length); // Calculates the distance between customers based on the max amount of customers
        newOrder.transform.position = transform.position + Vector3.right * ordersSlotIndex * distance; // Calculates and sets the position of the orders
        newOrder.transform.parent = transform;
        newOrder.transform.rotation = transform.rotation;

        GameManager.instance.orders[ordersSlotIndex] = newOrder; // Puts the order in the Orders array
    }

    float endlessModeStartTime = 0;
    void Update()
    {
        if (!GameManager.instance.isDayCyling && !GameManager.instance.isEndlessMode)
            return;
        
        float allowedCustomersAmount = 1;

        if (GameManager.instance.isEndlessMode)
        {
            if (endlessModeStartTime == 0)
                endlessModeStartTime = Time.time;
            
            float timeElapsedSinceEndlessModeStartTime = Time.time - endlessModeStartTime;

            if (timeElapsedSinceEndlessModeStartTime >= 60) // Changes difficulty to intermediate
                if (currentDifficulty == Difficulty.Easy)
                    currentDifficulty = Difficulty.Intermediate;

            if (timeElapsedSinceEndlessModeStartTime >= 120) // Changes difficulty to intermediate
                if (currentDifficulty == Difficulty.Intermediate)
                    currentDifficulty = Difficulty.Hard;


            switch (currentDifficulty)
            {
                case Difficulty.Easy:
                    allowedCustomersAmount = 1;
                    break;
                case Difficulty.Intermediate:
                    allowedCustomersAmount = 2;
                    break;
                case Difficulty.Hard:
                    allowedCustomersAmount = 3;
                    break;
                default:
                    allowedCustomersAmount = 1;
                    break;
            }
        }
        else
        {
            float a = GetCurrentOrders();
            float b = GameManager.instance.orders.Length;
            allowedCustomersAmount = a / b;
        }

        float currentDayTime = GameManager.instance.maxDayTime - GameManager.instance.dayTimeLeft;

        //Debug.Log(percentage);
        //Debug.Log(GetCurrentOrders() + " / " + GameManager.instance.orders.Length);
        //Debug.Log(GetCurrentOrders() / GameManager.instance.orders.Length);
        //Debug.Log(GameManager.instance.orders.Length);
        //Debug.Log(GameManager.instance.maxDayTime);
        //Debug.Log(GetCurrentOrders() / GameManager.instance.orders.Length * GameManager.instance.maxDayTime);

        if (Time.time >= nextOrderTime)
        {
            if (GetCurrentOrders() == 0)
            {
                CreateOrder();
                UpdateNextOrderTime();
            }
            else if (GameManager.instance.isEndlessMode && GetCurrentOrders() < allowedCustomersAmount) // Endless mode
            {
                CreateOrder();
                UpdateNextOrderTime();
            }
            else if (currentDayTime >= allowedCustomersAmount * GameManager.instance.maxDayTime) // Not endless mode
            {
                CreateOrder();
                UpdateNextOrderTime();
            }
            else
            {
                UpdateNextOrderTime();
            }
        }

        /*
        if (Time.time >= nextOrderTime) // Checks if should create new order
        {
            CreateOrder();
            UpdateNextOrderTime();
        }
        */
    }

    void Awake()
    {
        SetCompletedOrders();
    }
}
