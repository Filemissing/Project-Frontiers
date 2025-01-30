using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(AudioSource))]
public class Plate : Carryable
{
    AudioSource audioSource;
    public List<Ingredient> ingredients = new List<Ingredient>();
    public List<Recipe> validRecipes = new List<Recipe>();
    [SerializeField] Ingredient heldIngredient;
    public override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
        validRecipes = new List<Recipe>(GameManager.instance.recipes);
    }

    public int positionOnCounter;
    public override void InteractLeft()
    {
        if (GameManager.instance.playerCarry.carryingObject == null)
        { 
            // pickup the plate
            base.InteractLeft();
        }

        if (GetCarryingObject() == false) return;

        if (UpdateValidRecipes() == false) return;
        
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
        else if(GameManager.instance.playerCarry.carryingObject.TryGetComponent<FryBasket>(out FryBasket basket))
        {
            heldIngredient = basket.ingredient;// if the player is carrying a frybasket
        }
        else return false;

        if (heldIngredient != null) ingredients.Add(heldIngredient); // add heldingredient to ingredients to account for it in recipecheck
        return true;
    }

    bool UpdateValidRecipes()
    {
        List<Recipe> invalidRecipes = new List<Recipe>();
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
            if (!recipeIsPossible && (validRecipes.Count - invalidRecipes.Count) > 1)
            {
                invalidRecipes.Add(recipe);
            }
            else if (!recipeIsPossible && (validRecipes.Count - invalidRecipes.Count) <= 1)
            {
                ingredients.RemoveAt(ingredients.Count - 1); // remove the last entry aka the held ingredient
                return false;
            }
        }

        foreach (Recipe recipe in invalidRecipes)
        {
            validRecipes.Remove(recipe); // only remove invalid recipe's if at least 1 valid recipe was found
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
        // Most disgusting code in the game :(
        if (requirements.ingredient.GetType() == ingredient.GetType())
            if(requirements.isCooked == ingredient.isCooked)
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
        else if(GameManager.instance.playerCarry.carryingObject.TryGetComponent<FryBasket>(out FryBasket basket))
        {
            heldIngredient = basket.ingredient;// if the player is carrying a frybasket
            basket.ingredient = null;
        }
        heldIngredient.transform.SetParent(transform);
        heldIngredient.transform.position = transform.position;
        heldIngredient = null;
    }

    void CheckRecipeCompletion()
    {
        // please forgive me lord for this quadruple indentation
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
                return;
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
        audioSource.Play();
    }

    public override void Update()
    {
        base.Update();
        DisableIngredientHitBoxes(); // They were stealing the clicks
    }

    void DisableIngredientHitBoxes()
    {
        foreach (Ingredient ingredient in ingredients)
        {
            if (ingredient != null)
            {
                ingredient.GetComponent<Collider>().enabled = false; 
            }
        }
    }
}