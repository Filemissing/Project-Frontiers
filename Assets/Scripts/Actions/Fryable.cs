using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Ingredient))]
public class Fryable : MonoBehaviour
{
    Ingredient ingredient;
    private void Awake()
    {
        ingredient = GetComponent<Ingredient>();
    }

    public int fryTime;
    public int burnTime;
    float fryTimeCounter;
    public Material friedMaterial;
    public Material burntMaterial;
    public virtual void Fry()
    {
        fryTimeCounter += Time.deltaTime;

        if (fryTimeCounter >= fryTime)
        {
            ingredient.meshRenderer.material = friedMaterial; //change the material when fried
            ingredient.isFried = true;
        }

        if (fryTimeCounter >= burnTime)
        {
            ingredient.meshRenderer.material = burntMaterial; //change the material when burnt
            ingredient.isFried = false;
            ingredient.isBurnt = true;
        }
    }
}
