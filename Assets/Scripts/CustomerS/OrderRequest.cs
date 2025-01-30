using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class OrderRequest : MonoBehaviour
{
    public AudioClip satisfiedClip;
    public AudioClip dissatisfiedClip;
    public CompletedOrder order;
    public float maxTime;
    public float timeLeft;

    public ParticleSystem particleSystem;

    CompletedOrder GetCompletedOrder()
    {
        CompletedOrder completedOrder = null;

        if (GameManager.instance.playerCarry.carryingObject != null)
            if (GameManager.instance.playerCarry.carryingObject.TryGetComponent<Plate>(out Plate plate))
                if (GameManager.instance.playerCarry.carryingObject.transform.childCount != 0)
                    GameManager.instance.playerCarry.carryingObject.transform.GetChild(0).TryGetComponent<CompletedOrder>(out completedOrder);

        return completedOrder;
    }
    
    Message GetMessage(int rating)
    {
        Message chosenMessage;
        List<Message> possibleMessages = new List<Message>();

        for (int i = 0; i < GameManager.instance.messageHandler.reviews.Length; i++)
        {
            Message message = GameManager.instance.messageHandler.reviews[i];

            if (message.rating == rating)
                possibleMessages.Add(message);
        }

        if (possibleMessages.Count == 0)
            return GameManager.instance.messageHandler.reviews[0];
        
        chosenMessage = possibleMessages[Random.Range(0, possibleMessages.Count)];
        return chosenMessage;
    }

    float maxRating = 5f;
    void CorrectOrder()
    {
        OrderRequest[] orders = GameManager.instance.orders;
        for (int i = 0; i < orders.Length; i++) // Removes the OrderRequest from the Orders list
        {
            if (orders[i] == this)
                orders[i] = null;
        }

        Destroy(GameManager.instance.playerCarry.carryingObject); // destroy the plate with the order
        Destroy(gameObject); // destroy the customer
        GameManager.instance.ratings.Add(maxRating);
        GameManager.instance.messageHandler.SayMessage(GetMessage((int)maxRating));
        Instantiate(particleSystem.gameObject, transform.position, Quaternion.identity); // instantiate despawn effect
        AudioSource.PlayClipAtPoint(satisfiedClip, transform.position);
        Debug.Log("The CompletedOrder is correct.");
    }

    void IncorrectOrder()
    {
        maxRating -= .5f;
        maxRating = Mathf.Clamp(maxRating, 2, 5);
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

        /*
        Debug.Log(completedOrder);
        string prefabPath0 = AssetDatabase.GetAssetPath(completedOrder);
        string prefabPath1 = AssetDatabase.GetAssetPath(order.gameObject);

        Debug.Log(prefabPath0);
        Debug.Log(prefabPath1);

        if (prefabPath0 == prefabPath1) // Checks if they have the same prefab
            isCorrectCompletedOrder = true;
        */


        if (completedOrder.sprite == order.sprite) // Checks if sprites are the same, if they are it uses the same prefab
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

    void OutOfTime()
    {
        GameManager.instance.ratings.Add(1f);
        GameManager.instance.messageHandler.SayMessage(GetMessage(1));
        Destroy(gameObject);
        Instantiate(particleSystem, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(dissatisfiedClip, transform.position);
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0)
            OutOfTime();
    }

    void Awake()
    {
        timeLeft = maxTime;
    }
}

//Component[] components = child.GetComponents<Component>();
//System.Type baseType = typeof(Ingredient);

//foreach (Component component in components)
//{
//    if (component == null)
//        continue;

//    if (component.GetType().IsSubclassOf(baseType))
//    {
//        Debug.Log(component.GetType().Name);
//    }
//}
