using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject player;
    public Carry playerCarry;
    public Recipe[] recipes;
    public CompletedOrder[] completedOrders;
    public List<OrderRequest> orders = new List<OrderRequest>();
    public OrderRequest orderRequest;
    public GameObject ordersObject;

    public float timeBetweenOrdersMin = 1;
    public float timeBetweenOrdersMax = 5;
    float nextOrderTime;

    void Reset()
    {
        instance = this;
    }

    void Awake()
    {
        playerCarry = player.GetComponent<Carry>();
    }

    void UpdateNextOrderTime()
    {
        nextOrderTime = Time.time + Random.Range(timeBetweenOrdersMin, timeBetweenOrdersMax);
    }

    void CreateOrder()
    {
        OrderRequest newOrder = Instantiate<OrderRequest>(orderRequest);
        newOrder.order = completedOrders[Random.Range(0, recipes.Length)];
        newOrder.transform.parent = ordersObject.transform;

        orders.Add(newOrder);
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
