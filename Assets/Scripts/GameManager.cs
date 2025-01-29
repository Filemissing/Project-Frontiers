using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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

    [Header("Day Time")]
    public bool isEndlessMode = false;
    public bool isDayCyling;
    public float maxDayTime = 210;
    public float dayTimeLeft;

    [Header("Recipe Popup")]
    public float recipePopupRange = 2.5f;
    public RecipePopup currentRecipePopup = null;
    public List<RecipePopup> recipePopups = new List<RecipePopup>();

    [Header("Message Handler")]
    public MessageHandler messageHandler;
    public bool isDialoguing;

    [Header("Rating")]
    public float rating = 0;
    public List<float> ratings = new List<float>();

    void DayEnd()
    {
        dayTimeLeft = 0;
        //Time.timeScale = 0;
    }

    void Awake()
    {
        playerCarry = player.GetComponent<Carry>();
        playerController = player.GetComponent<PlayerController>();
        instance = this;
        dayTimeLeft = maxDayTime;

        if (!isEndlessMode)
            messageHandler.SayMessage(messageHandler.dialogues[0]);
    }

    void UpdateDayTime()
    {
        if (isEndlessMode)
            return;

        if (!isDayCyling)
            return;

        if (dayTimeLeft <= 0)
            return;

        dayTimeLeft -= Time.deltaTime;
        dayTimeLeft = Mathf.Clamp(dayTimeLeft, 0, maxDayTime);

        if (dayTimeLeft == 0)
            DayEnd();
    }

    void UpdateCurrentRecipePopup()
    {
        List<RecipePopup> recipePopupsCandidates = new List<RecipePopup>();
        RecipePopup closestRecipePopup = null;
        float closestRecipePopupDistance = 10000;


        for (int i = 0; i < recipePopups.Count; i++) // Makes RecipePopupsCandidates list
        {
            RecipePopup recipePopup = recipePopups[i];
            bool willGetAdded = true;

            if (playerCarry.carryingObject) // Checks if player is holding the plate containing this RecipePopup
                if (playerCarry.carryingObject.transform.childCount > 0)
                    if (playerCarry.carryingObject.transform.GetChild(0).GetComponent<RecipePopup>())
                        if (playerCarry.carryingObject.transform.GetChild(0).GetComponent<RecipePopup>() == recipePopup)
                            willGetAdded = false;


            if (willGetAdded)
                recipePopupsCandidates.Add(recipePopup);
        }


        for (int i = 0; i < recipePopupsCandidates.Count; i++) // Gets closest RecipePopup
        {
            RecipePopup recipePopup = recipePopupsCandidates[i];
            float distance = (recipePopup.transform.position - playerController.transform.position).magnitude;

            if (distance < closestRecipePopupDistance)
            {
                closestRecipePopup = recipePopup;
                closestRecipePopupDistance = distance;
            }
        }

        if (closestRecipePopupDistance <= recipePopupRange) // Checks if within range
            currentRecipePopup = closestRecipePopup;
        else
            currentRecipePopup = null;
    }

    void UpdateIsDialoguing()
    {
        isDialoguing = messageHandler.isDialogueVisible;
    }

    void UpdateRating()
    {
        if (ratings.Count == 0)
        {
            rating = 5;
            return;
        }

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
        if (isEndlessMode && ratings.Count != 0 && rating < 4) DayEnd();

        playerController.canMove = !isDialoguing; // Disables player movement while dialoguing

        UpdateDayTime();
        UpdateCurrentRecipePopup();
        UpdateIsDialoguing();
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
        Debug.Log("enterUIMode");
        playerController.enabled = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void ExitUIMode()
    {
        Debug.Log("ExitUIMode");
        playerController.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
    }
}