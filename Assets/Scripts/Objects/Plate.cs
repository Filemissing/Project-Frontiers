using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class Plate : Carryable
{
    [SerializeField] List<Ingredient> ingredients = new List<Ingredient>();
    [SerializeField] List<Recipe> validRecipes = new List<Recipe>();
    [SerializeField] Ingredient heldIngredient;
    private void Awake()
    {
        validRecipes = new List<Recipe>(GameManager.instance.recipes);
    }
    public override void InteractLeft()
    {
        base.InteractLeft();

        if (GetCarryingObject() == false) return;

        if(UpdateValidRecipes() == false) return;
        
        RemoveHeldObject();

        CheckRecipeCompletion();
    }

    bool GetCarryingObject()
    {
        if (GameManager.instance.playerCarry.carryingObject == null) return false;

        if (GameManager.instance.playerCarry.carryingObject.TryGetComponent<Ingredient>(out heldIngredient))
        {
            // if player is carrying an ingredient
        } 
        else if (GameManager.instance.playerCarry.carryingObject.TryGetComponent<Pan>(out Pan pan))
        {
            heldIngredient = pan.ingredient;// if the player is carrying a pan
        }
        else return false;

        if (heldIngredient != null) ingredients.Add(heldIngredient);
        return true;
    }

    bool UpdateValidRecipes()
    {
        retry:
        foreach (Recipe recipe in validRecipes)
        {
            bool recipeIsPossible = true;
            foreach (Ingredient ingredient in ingredients)
            {
                if (CheckForIngredient(recipe, ingredient) == false)
                {
                    recipeIsPossible = false;
                    break;
                }
            }
            if (!recipeIsPossible && validRecipes.Count > 1)
            {
                validRecipes.Remove(recipe);
                goto retry;
            }
            else if (!recipeIsPossible && validRecipes.Count <= 1)
            {
                ingredients.RemoveAt(ingredients.Count - 1); // remove the last entry aka the held ingredient
                return false;
            }
        }
        return true;
    }
    bool CheckForIngredient(Recipe recipe, Ingredient ingredient)
    {
        foreach (IngredientRequirements i in recipe.ingredients)
        {
            if(CompareIngredientToIngredientRequirements(ingredient, i)) return true;
        }
        return false;
    }
    bool CompareIngredientToIngredientRequirements(Ingredient ingredient, IngredientRequirements requirements)
    {
        if (requirements.ingredient.GetType() == ingredient.GetType())
            if (requirements.isFried == ingredient.isFried)
                if (requirements.isBurnt == ingredient.isBurnt)
                    if (requirements.isChopped == ingredient.isChopped)
                        return true;
        return false;
    }
    
    void RemoveHeldObject()
    {
        if (GameManager.instance.playerCarry.carryingObject.TryGetComponent<Ingredient>(out heldIngredient))
        {
            GameManager.instance.playerCarry.carryingObject = null;// if player is carrying an ingredient
        }
        else if (GameManager.instance.playerCarry.carryingObject.TryGetComponent<Pan>(out Pan pan))
        {
            heldIngredient = pan.ingredient;// if the player is carrying a pan
            pan.ingredient = null;
        }     
        heldIngredient.transform.SetParent(transform);
        heldIngredient.transform.position = transform.position;
    }

    void CheckRecipeCompletion()
    {
        foreach(Recipe recipe in validRecipes)
        {
            bool recipeIsComplete = true;
            foreach(IngredientRequirements requirements in recipe.ingredients)
            {
                bool ingredientIsPresent = false;
                foreach (Ingredient ingredient in ingredients)
                {
                    if (CompareIngredientToIngredientRequirements(ingredient, requirements))
                    {
                        ingredientIsPresent = true;
                        break;
                    }
                }
                if (!ingredientIsPresent)
                {
                    recipeIsComplete = false;
                    break;
                }
            }

            if (recipeIsComplete)
            {
                ReplaceWithRecipeResult(recipe);
            }
        }
    }
    void ReplaceWithRecipeResult(Recipe recipe)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        Instantiate(recipe.endResult, transform);
    }
}