using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class RecipePopup : MonoBehaviour
{
    [SerializeField] Plate plate;
    [SerializeField] GameObject recipePanel;
    [SerializeField] Transform recipePanelParent;
    [SerializeField] GameObject ingredientPanel;
    [SerializeField] GameObject conditionTextLabel;

    [Header("TESTING")]
    [SerializeField] Recipe testRecipe;


    Sprite GetCompletedRecipeSprite(Recipe recipe)
    {
        return recipe.endResult.GetComponent<CompletedOrder>().sprite;
    }

    void CreateCondition(string conditionString, Transform conditionsTextLabelParent)
    {
        GameObject newConditionTextLabel = Instantiate<GameObject>(conditionTextLabel);
        newConditionTextLabel.transform.SetParent(conditionsTextLabelParent, false);
        newConditionTextLabel.GetComponent<TMP_Text>().text = "- " + conditionString;
    }

    void CreateIngredientPanel(IngredientRequirements ingredientRequirements, Transform ingredientPanelParent)
    {
        GameObject newIngredientPanel = Instantiate<GameObject>(ingredientPanel);
        newIngredientPanel.transform.SetParent(ingredientPanelParent, false);

        Image ingredientImageLabel = newIngredientPanel.transform.GetChild(0).GetComponent<Image>();
        ingredientImageLabel.sprite = ingredientRequirements.ingredient.icon;

        TMP_Text ingredientTextLabel = newIngredientPanel.transform.GetChild(1).GetComponent<TMP_Text>();
        ingredientTextLabel.text = ingredientRequirements.ingredient.name;

        Transform conditionsTextLabelParent = newIngredientPanel.transform.GetChild(2);

        if (ingredientRequirements.isChopped)
            CreateCondition("Chopped", conditionsTextLabelParent);

        if (ingredientRequirements.isFried)
            CreateCondition("Fried", conditionsTextLabelParent);

        if (ingredientRequirements.isCooked)
            CreateCondition("Cooked", conditionsTextLabelParent);

        if (ingredientRequirements.isBurnt)
            CreateCondition("Burnt", conditionsTextLabelParent);
    }

    void CreateRecipePanel(Recipe recipe)
    {
        GameObject newRecipePanel = Instantiate<GameObject>(recipePanel);
        newRecipePanel.transform.SetParent(recipePanelParent, false);

        RectTransform newRecipePanelRectTransform = newRecipePanel.GetComponent<RectTransform>(); // This is added cuz unity UI prefabs buggy
        newRecipePanelRectTransform.localScale = Vector3.one;
        newRecipePanelRectTransform.localPosition = Vector3.up * 1.5f;
        newRecipePanelRectTransform.localRotation = quaternion.identity;

        Image completedRecipeImageLabel = newRecipePanel.transform.GetChild(0).GetComponent<Image>();
        completedRecipeImageLabel.sprite = GetCompletedRecipeSprite(recipe);

        Transform ingredientPanelParent = newRecipePanel.transform.GetChild(1);

        for (int i = 0; i < recipe.ingredients.Length; i++)
        {
            IngredientRequirements ingredientRequirements = recipe.ingredients[i];
            CreateIngredientPanel(ingredientRequirements, ingredientPanelParent);
        }
    }

    void Awake()
    {
        CreateRecipePanel(testRecipe);
    }
}
