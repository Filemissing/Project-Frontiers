using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IngredientCombiner : MonoBehaviour
{
    List<Ingredient> ingredients;
    List<Recipe> validRecipes = new List<Recipe>(GameManager.instance.recipes);
    Ingredient heldIngredient;
    public void Interacted()
    {
        if(!GameManager.instance.playerCarry.carryingObject.TryGetComponent<Ingredient>(out heldIngredient)) return; // if player isn't carrying an ingredient
     
        ingredients.Add(heldIngredient);

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
    }
}