using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderRequest : MonoBehaviour
{
    public CompletedOrder order;

    CompletedOrder GetCompletedOrder()
    {
        CompletedOrder completedOrder = null;
        
        if (GameManager.instance.playerCarry)
            if (GameManager.instance.playerCarry.carryingObject)
                if (GameManager.instance.playerCarry.carryingObject.TryGetComponent<CompletedOrder>(out CompletedOrder component))
                    completedOrder = component;

        return completedOrder;
    }

    void CorrectOrder()
    {
        OrderRequest[] orders = GameManager.instance.orders;
        for (int i = 0; i < orders.Length; i++) // Removes the OrderRequest from the Orders list
        {
            if (orders[i] == this)
                orders[i] = null;
        }

        Destroy(GameManager.instance.playerCarry.carryingObject);
        Destroy(gameObject);
        Debug.Log("The CompletedOrder is correct.");
    }

    void IncorrectOrder()
    {
        Debug.Log("The CompletedOrder is incorrect.");
    }

    void CheckOrder()
    {
        CompletedOrder completedOrder = GetCompletedOrder();
        bool isCorrectCompletedOrder = false;

        if (completedOrder == null)
        {
            Debug.Log("CompletedOrder not found.");
            return;
        }

        if (completedOrder.name == order.name) // Currently checks by name, change when needed.
            isCorrectCompletedOrder = true;

        if (isCorrectCompletedOrder)
            CorrectOrder();
        else
            IncorrectOrder();
    }

    void InteractLeft()
    {
        CheckOrder();
    }
}