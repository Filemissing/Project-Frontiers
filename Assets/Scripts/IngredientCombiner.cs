using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IngredientCombiner : MonoBehaviour
{
    List<Ingredient> ingredients;
    List<Recipe> validRecipes;
    Ingredient heldIngredient;
    private void Awake()
    {
        validRecipes = new List<Recipe>(GameManager.instance.recipes);
    }
    public void InteractLeft()
    {
        Debug.Log("interacted with combining station");
        Debug.Log(GameManager.instance);
        if(GameManager.instance.playerCarry)
            if(GameManager.instance.playerCarry.carryingObject)
                if(!GameManager.instance.playerCarry.carryingObject.TryGetComponent<Ingredient>(out heldIngredient)) 
                    return; // if player isn't carrying an ingredient
     
        if(heldIngredient) ingredients.Add(heldIngredient);

        foreach (Recipe recipe in validRecipes)
        {
            bool recipeIsPossible = true;
            foreach(Ingredient ingredient in ingredients)
            {
                if (!recipe.ingredients.Contains(ingredient.ToIngredientRequirement()))
                {
                    recipeIsPossible = false;
                    break;
                }
            }
            if (!recipeIsPossible && validRecipes.Count > 1)
            {
                validRecipes.Remove(recipe);
            }
            else if (validRecipes.Count <= 1)
            {
                ingredients.RemoveAt(ingredients.Count - 1); // remove the last entry aka the held ingredient
                return;
            } 
        }
        GameManager.instance.playerCarry.carryingObject = null;
        heldIngredient.transform.SetParent(gameObject.transform);
        heldIngredient.transform.position = Vector3.zero;
    }
}