using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject player;
    PlayerController playerController;
    public Carry playerCarry;
    public Recipe[] recipes;
    public OrderRequest[] orders;

    [Header("Rating")]
    public float rating = 0;
    public List<float> ratings = new List<float>();

    void Awake()
    {
        playerCarry = player.GetComponent<Carry>();
        playerController = player.GetComponent<PlayerController>();
        instance = this;
    }

    void UpdateRating()
    {
        float totalRatings = 0;
        for (int i = 0; i < ratings.Count; i++)
        {
            totalRatings += ratings[i];
        }

        float averageRating = totalRatings / ratings.Count;
        rating = averageRating;
    }

    void Update()
    {
        if(instance == null) instance = this;
        UpdateRating();
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

    public void EnterUIMode()
    {
        playerController.enabled = false;
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void ExitUIMode()
    {
        playerController.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
    }
}