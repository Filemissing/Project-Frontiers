using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fridge : MonoBehaviour
{
    public GameObject fridgeInterface;
    GameObject ingredientArea;
    public Transform spawnPosition;
    public Ingredient[] ingredients;

    public void InteractLeft()
    {
        fridgeInterface.SetActive(true);
        GameManager.instance.EnterUIMode();
    }

    public void SelectIngredient(int index)
    {
        GameObject spawnedObject = Instantiate(ingredients[index].gameObject, spawnPosition);
        
        if(GameManager.instance.playerCarry.carryingObject == null)
        {
            GameManager.instance.playerCarry.carryingObject = spawnedObject;
        }

        Exit();
    }

    public void Exit()
    {
        GameManager.instance.ExitUIMode();
        fridgeInterface.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            fridgeInterface.SetActive(false);
        }
        else if(GameManager.instance.dayTimeLeft == 0)
        {
            fridgeInterface.SetActive(false);
        }
        if (fridgeInterface.activeSelf)
        {
            GameManager.instance.EnterUIMode();
        }
    }
}