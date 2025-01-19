using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Ingredient))]
public class Cookable : MonoBehaviour
{
    Ingredient ingredient;
    private void Awake()
    {
        ingredient = GetComponent<Ingredient>();
    }

    public float cookTime;
    public float burnTime;
    float cookTimeCounter;
    public Material cookedMaterial;
    public Material burntMaterial;
    public virtual void Cook()
    {
        cookTimeCounter += Time.deltaTime;

        if (cookTimeCounter >= cookTime)
        {
            ingredient.meshRenderer.material = cookedMaterial; //change the material when fried
            ingredient.isCooked = true;
        }

        if (cookTimeCounter >= burnTime)
        {
            ingredient.meshRenderer.material = burntMaterial; //change the material when burnt
            ingredient.isCooked = false;
            ingredient.isBurnt = true;
        }
    }
}
