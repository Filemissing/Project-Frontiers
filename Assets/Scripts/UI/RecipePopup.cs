using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class RecipePopup : MonoBehaviour
{
    [SerializeField] Plate plate;
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject recipePanel;
    [SerializeField] Transform recipePanelParent;
    [SerializeField] GameObject ingredientPanel;
    [SerializeField] GameObject conditionTextLabel;

    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] RectTransform recipesPanelRectTransform;

    List<GameObject> currentRecipePanels = new List<GameObject>();

    [Header("Visibility")]
    [SerializeField] float maxDistance = 2.5f;

    [SerializeField] bool isVisible = false;
    [SerializeField] float transitionTime = .15f;


    [Header("Measurements")]
    [SerializeField] Vector2 ingredientPanelDefaultSize = new Vector2(.65f, .23f);
    [SerializeField] float ingredientPanelMargin = .065f;

    [SerializeField] Vector2 recipePanelDefaultSize = new Vector2(.7f, .735f);
    [SerializeField] float recipePanelMargin = .065f;
    [SerializeField] float recipePanelIngredientPanelMargin = .26f; //.345f;



    bool previousIsVisible = false;
    void VisibilityChange()
    {
        if (isVisible != previousIsVisible)
        {
            if (isVisible)
            {
                canvasGroup.DOFade(1, transitionTime);
                recipesPanelRectTransform.DOScale(Vector3.one, transitionTime);
            }
            else
            {
                canvasGroup.DOFade(0, transitionTime);
                recipesPanelRectTransform.DOScale(Vector3.one * .3f, transitionTime);
            }
        }

        previousIsVisible = isVisible;
    }

    void LookAtPlayer()
    {
        if (GameManager.instance.player == null)
            return;

        Vector3 direction = GameManager.instance.player.transform.position - canvas.transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 eulerAngles = lookRotation.eulerAngles;
        canvas.transform.eulerAngles = new Vector3(0, eulerAngles.y + 180, 0);
    }

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

    void CreateIngredientPanel(IngredientRequirements ingredientRequirements, Transform ingredientPanelParent, Recipe recipe)
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
        

        // Changing Sizes
        int conditionCount = conditionsTextLabelParent.childCount;

        newIngredientPanel.GetComponent<RectTransform>().sizeDelta = new Vector2
        (
            ingredientPanelDefaultSize.x,
            ingredientPanelDefaultSize.y + ingredientPanelMargin * (conditionCount - 1)
        );

        ingredientPanelParent.parent.GetComponent<RectTransform>().sizeDelta = new Vector2
        (
            recipePanelDefaultSize.x,
            recipePanelDefaultSize.y + recipePanelMargin * (conditionCount - 1) + recipePanelIngredientPanelMargin * (recipe.ingredients.Length - 1)
            //recipePanelDefaultSize.y + recipePanelMargin * (conditionCount - 1) + recipePanelIngredientPanelMargin * (recipe.ingredients.Length - 1) 
        );
    }

    void CreateRecipePanel(Recipe recipe)
    {
        GameObject newRecipePanel = Instantiate<GameObject>(recipePanel);
        newRecipePanel.transform.SetParent(recipePanelParent, false);
        newRecipePanel.name = recipe.name;

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
            CreateIngredientPanel(ingredientRequirements, ingredientPanelParent, recipe);
        }

        currentRecipePanels.Add(newRecipePanel);
    }

    void RemoveRecipePanel(GameObject thisRecipePanel)
    {
        Destroy(thisRecipePanel);
        currentRecipePanels.Remove(thisRecipePanel);
    }



    void Update()
    {
        LookAtPlayer();
        VisibilityChange();


        if (GameManager.instance.currentRecipePopup == this)
            isVisible = true;
        else
            isVisible = false;
        
        //float distance = (GameManager.instance.player.transform.position - plate.transform.position).magnitude;
        //isVisible = distance <= maxDistance;


        for (int i = 0; i < plate.validRecipes.Count; i++) // Adding RecipePanels
        {
            Recipe thisRecipe = plate.validRecipes[i];
            bool currentRecipePanelFound = false;

            for (int ii = 0; ii < currentRecipePanels.Count; ii++)
            {
                GameObject thisRecipePanel = currentRecipePanels[ii];
                if (thisRecipePanel.name == thisRecipe.name)
                    currentRecipePanelFound = true;
            }

            if (!currentRecipePanelFound)
            {
                CreateRecipePanel(thisRecipe);
            }
        }

        for (int i = 0; i < currentRecipePanels.Count; i++) // Removing RecipePanels
        {
            GameObject thisRecipePanel = currentRecipePanels[i];
            bool currentRecipeFound = false;

            for (int ii = 0; ii < plate.validRecipes.Count; ii++)
            {
                Recipe thisRecipe = plate.validRecipes[ii];
                if (thisRecipe.name == thisRecipePanel.name)
                    currentRecipeFound = true;
            }

            if (!currentRecipeFound)
            {
                RemoveRecipePanel(thisRecipePanel);
            }
        }
    }



    void Awake()
    {
        GameManager.instance.recipePopups.Add(this); // Adds this RecipePopup to the public list

        canvasGroup.alpha = 0;
        recipesPanelRectTransform.localScale = Vector3.one * .3f;
    }


    void OnDestroy()
    {
        GameManager.instance.recipePopups.Remove(this); // Removes this RecipePopup from the public list
    }
}
