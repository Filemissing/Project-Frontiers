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
    public OrderRequest[] orders;

    void Awake()
    {
        playerCarry = player.GetComponent<Carry>();
        instance = this;
    }

    void Update()
    {
        if(instance == null) instance = this;
    }

    public bool TakeCarryingObject<T>(GameObject newParent, out T returnComponent)
    {
        if (playerCarry.carryingObject == null)
        {
            returnComponent = default(T);
            return false;
        }

        if(playerCarry.carryingObject.TryGetComponent<T>(out T component))
        {
            GameObject gameObject = playerCarry.carryingObject;
            gameObject.transform.SetParent(newParent.transform);
            gameObject.transform.position = newParent.transform.position;

            playerCarry.carryingObject = null;

            returnComponent = component;
            return true;
        }

        returnComponent = default(T);
        return false;
    }
}