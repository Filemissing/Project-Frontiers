using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[System.Serializable]
public class Ingredient : MonoBehaviour
{
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    protected virtual void Awake()
    {
        meshFilter = gameObject.GetComponent<MeshFilter>();
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
    }

    public bool isFried;
    public bool isBurnt;
    public bool isChopped;

    public IngredientRequirements ToIngredientRequirement()
    {
        IngredientRequirements ingredientvalues = new IngredientRequirements
        {
            ingredient = this,
            isChopped = isChopped,
            isFried = isFried,
            isBurnt = isBurnt
        };
        return ingredientvalues;
    }
}